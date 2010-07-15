using System;
using System.Threading;
using MagnumHouseLib;
using MagnumHouse;

namespace Client
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			NetworkClient client = new NetworkClient("127.0.0.1");
			client.Connect();
			Thread.Sleep(10);
			client.SendMessage(new GangsterMessage(new Vector2f(1,1)));
			while(true) {Thread.Sleep(10);}
		}
	}
}
