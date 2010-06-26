using System;
using System.Threading;
using MagnumHouseLib;

namespace Client
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			NetworkClient client = new NetworkClient("192.168.1.100");
			while(true) {Thread.Sleep(10);}
		}
	}
}
