using System;

using System.Collections.Generic;

namespace MagnumHouseLib
{
	public class NetworkHouseServer : NetworkServer
	{
		Dictionary<Type, Dictionary<int, IUpdateable>> stuff = new Dictionary<Type, Dictionary<int, IUpdateable>>();
		
		IObjectCollection m_objectCollection;
		
		public NetworkHouseServer() {
			m_objectCollection = new ObjectHouse();
		}
		
		protected override void HandleMessage (GenericMessage message)
		{
			var typeDic = stuff[message.ContentType];
			if (!typeDic.ContainsKey(message.Content.Id)) {
				typeDic[message.Content.Id] = MakeNew(message.ContentType, message.Content);
			}
			message.Content.ApplyTo(typeDic[message.Content.Id]);
		}
		
		private IUpdateable MakeNew(Type type, MessageContent content) {
			if (type == typeof(Gangster)) {
				return MakeNewGangster(content);
			} else if (type == typeof(Slug)) {
				return MakeNewSlug(content);
			}
			return null;
		}
		
		private Gangster MakeNewGangster(MessageContent content) {
			Gangster gangster = new Gangster(m_objectCollection);
			return gangster;
		}
		
		private Slug MakeNewSlug(MessageContent content) {
			Gangster gangster = (Gangster)stuff[typeof(Gangster)][((SlugMessageContent)content).GangsterId];
			Slug slug = new Slug(gangster.Magnum);
			return slug;
		}
	}
}
