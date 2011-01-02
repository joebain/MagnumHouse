using System;
using System.Threading;
using MagnumHouseLib;
using MagnumHouseLib;

namespace Server
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine("server");
			NetworkServer server = new NetworkServer();
			server.Start();
			
			var sl = new ServerLevel(server);
			var game = new Game();
			Game.Zoom = 0.2f;
			game.SetLevels(new [] {sl});
			game.Setup();
			game.Run();
		}
	}
}
