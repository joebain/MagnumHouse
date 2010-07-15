using System;

using System.Net.Sockets;

namespace MagnumHouseLib
{
	public class Visage {
		public GangsterVisage gangster;
		public TcpClient client;
		public const int bufferSize = 100;
		public byte[] readBuffer = new byte[bufferSize];
	}
}
