
using System;
using System.Collections.Generic;
using MagnumHouseLib;

namespace MagnumHouseLib
{
	public class NetworkHero : NetworkClient, IUpdateable
	{
		Hero hero;
		ObjectHouse house;
		Dictionary<Int32, GangsterVisage> clientDetails = new Dictionary<Int32, GangsterVisage>();
		Dictionary<int, GhostGangster> ghost_gangsters = new Dictionary<int, GhostGangster>();
		Int32 id;
		public int Id {get { return id;} }
		bool hasId = false;
		
		public NetworkHero (Hero _gangster, string serverAddress, ObjectHouse _house) : base(serverAddress)
		{
			hero = _gangster;
			house = _house;
		}
		
		public void Update(float _delta) {
			if (hasId) {
				var message = new GangsterMessage(id, hero.Position);
				SendMessage(message);
			}
		}
		
		bool dead = false;
		public bool Dead { get {return dead;}}
		
		public void Die() {
			Console.WriteLine("dying");
			if (hasId) {
				SendMessage(new ClientGoodbyeMessage(id));
			}
			dead = true;
		}
		
		protected override void HandleMessage ()
		{
//			if (GangsterMessage.SIs(readBuffer)) {
//				var gm = new GangsterMessage(readBuffer);
//				GangsterVisage visage;
//				if (!visages.TryGetValue(gm.Id, out visage) && gm.Id != id) {
//					Console.WriteLine("got new ghost, id: " + gm.Id);
//					visage = new GangsterVisage();
//					visages.Add(gm.Id, visage);
//					var gg = new GhostGangster(visage);
//					ghost_gangsters[gm.Id] = gg;
//					house.AddDrawable(gg);
//					house.AddUpdateable(gg);
//				}
//				visage.Receive(gm);
//			} else if (HelloMessage.SIs(readBuffer)) {
//				var hm = new ClientHelloMessage(readBuffer);
//				id = hm.Id;
//				hasId = true;
//				Console.WriteLine("got id " + id);
//			} else if (GoodbyeMessage.SIs(readBuffer)) {
//				var gm = new ClientGoodbyeMessage(readBuffer);
//				if (visages.ContainsKey(gm.Id) && ghost_gangsters.ContainsKey(gm.Id)) {
//					house.RemoveDrawable(ghost_gangsters[gm.Id]);
//					house.RemoveUpdateable(ghost_gangsters[gm.Id]);
//					visages.Remove(gm.Id);
//					ghost_gangsters.Remove(gm.Id);
//					Console.WriteLine("ghost left, id " + gm.Id);
//				}
//			}
		}
	}
}
