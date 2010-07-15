
using System;
using MagnumHouse;

namespace MagnumHouseLib
{
	public class NetworkHero : NetworkClient, IUpdateable
	{
		Hero hero;
		
		public NetworkHero (Hero _gangster, string serverAddress) : base(serverAddress)
		{
			hero = _gangster;
		}
		
		public void Update(float _delta) {
			var message = new GangsterMessage(hero.Position);
			SendMessage(message);
		}
		
		public bool Dead { get {return false;}}
	}
}
