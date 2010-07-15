
using System;
using MagnumHouse;

namespace MagnumHouseLib
{
	public class ServerLevel : Screen
	{
		NetworkServer server;
		
		public ServerLevel (NetworkServer _server)
		{
			server = _server;
			server.visage.GangsterAdded += HandleVisageGangsterAdded;
		}

		void HandleVisageGangsterAdded (GangsterVisage _gangster)
		{
			var gg = new GhostGangster(_gangster);
			m_house.AddDrawable(gg);
			m_house.AddUpdateable(gg);
			
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
