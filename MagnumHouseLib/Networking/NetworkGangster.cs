
using System;

using MagnumHouseLib;

namespace MagnumHouseLib
{
	public class NetworkGangster : NetworkClient
	{
		Gangster gangster;
		
		public NetworkGangster (Gangster _gangster, string serverAddress) : base (serverAddress)
		{
			gangster = _gangster;
		}
		
		protected override void HandleMessage ()
		{
			if (GangsterMessage.SIs(readBuffer)) {
				var gm = new GangsterMessage(readBuffer);
				Console.WriteLine("position is " + gm.Position);
			}
		}
	}
}
