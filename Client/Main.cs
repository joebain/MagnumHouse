using System;
using System.Threading;
using MagnumHouseLib;

namespace Client
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			NetworkClient client = new NetworkClient("127.0.0.1");
			client.Connect();
			while(true) {Thread.Sleep(10);}
		}
	}
}
