
using System;
using MagnumHouseLib;
using System.Collections.Generic;

namespace MagnumHouseLib
{
	public class ServerLevel : Screen
	{
		NetworkServer server;
		
		Dictionary<int, GhostGangster> gsters = new Dictionary<int, GhostGangster>();
		
		public ServerLevel (NetworkServer _server)
		{
			server = _server;
			server.World.GangsterAdded += HandleVisageGangsterAdded;
			server.World.GangsterRemoved += HandleVisageGangsterRemoved;
		}

		void HandleVisageGangsterAdded (GangsterVisage _gangster)
		{
			var gg = new GhostGangster(_gangster);
			Console.WriteLine("adding gster w id: {0}", _gangster.id);
			gsters[gg.Id] = gg;
			m_house.AddDrawable(gg);
			m_house.AddUpdateable(gg);
			
		}
		
		void HandleVisageGangsterRemoved (GangsterVisage _gangster) {
			GhostGangster gg;
			if (gsters.TryGetValue(_gangster.id, out gg)) {
				m_house.RemoveDrawable(gg);
				Console.WriteLine("removed a g pic, id {0}",_gangster.id);
			} else {
				Console.WriteLine("couldnt remove a g pic, id {0}",_gangster.id);
				Console.WriteLine("# gsters: {0}, ids:",gsters.Count);
				foreach (var kvp in gsters) {
					Console.Write("{0}, ", kvp.Key);
				}
				Console.WriteLine();
			}
		}
		
		public override void Setup(Game _game, UserInput _keyboard, ScreenMessage _message) {
			base.Setup(_game, _keyboard, _message);
			
			m_house = new ObjectHouse();
			
			m_house.AddUpdateable(server);
		}
		
		public override void Update(float _delta) {
			
		}
	}
}
