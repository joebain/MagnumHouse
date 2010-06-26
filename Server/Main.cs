using System;
using System.Threading;
using MagnumHouseLib;

namespace Server
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			NetworkServer server = new NetworkServer();
			
			while(true) {Thread.Sleep(10);}
		}
	}
}
