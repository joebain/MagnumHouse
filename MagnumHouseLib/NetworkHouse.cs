
using System;
using System.Collections.Generic;

namespace MagnumHouseLib
{
	public class NetworkHouse : NetworkClient
	{
		Dictionary<Type, Dictionary<Int32, IUpdateable>> stuff = new Dictionary<Type, Dictionary<Int32, IUpdateable>>();
		
		Queue<IUpdateable> waitingForId = new Queue<IUpdateable>();
		
		Int32 Id;
		
		NetworkHouse(string serverAddress) : base(serverAddress) {
			
		}
		
		public void AddUdateable(IUpdateable _updateable) {
			if (_updateable is Hero) {
				Hero hero = (Hero)_updateable;
				waitingForId.Enqueue(hero);
				SendMessage(new ClientHelloMessage());
			}
		}
		
		protected override void HandleMessage ()
		{
			if (ClientHelloMessage.SIs(readBuffer)) {
				int new_id = new ClientHelloMessage(readBuffer).Id;
				Id = new_id;
				Console.WriteLine("connected!");
			}
			if (HelloMessage.SIs(readBuffer)) {
				int new_id = new HelloMessage(readBuffer).Id;
				
				IUpdateable updateable = waitingForId.Dequeue();
				Type t = updateable.GetType();
				stuff[t][new_id] = updateable;
				if (updateable is Slug) {
					var slug = (Slug)updateable;
					SendMessage(new SlugMessage(new_id, slug.Position, slug.Speed, slug.Magnum.Owner.Id));
				} else if (updateable is Gangster) {
					var gangster = (Gangster)updateable;
					SendMessage(new GangsterMessage(new_id, gangster.Position));
				}
			} else if (GangsterMessage.SIs(readBuffer)) {
				var gm = new GangsterMessage(readBuffer);
				var gangster = (Gangster)stuff[typeof(Gangster)][gm.Id];
				gangster.Position = gm.Position;
			} else if (SlugMessage.SIs(readBuffer)) {
				var sm = new SlugMessage(readBuffer);
				var slug = (Slug)stuff[typeof(Slug)][sm.Id];
				slug.Position = sm.Position;
				slug.Speed = sm.Velocity;
			}
		}
	}
}
