using System;
using System.Threading;
using MagnumHouseLib;
using MagnumHouse;

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
			game.Zoom = 0.2f;
			game.Setup(new [] {sl});
			game.Run();
		}
	}
}
