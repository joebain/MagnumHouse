using System;
using System.Threading;
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
			while(true) {Thread.Sleep(10);}
		}
	}
}
