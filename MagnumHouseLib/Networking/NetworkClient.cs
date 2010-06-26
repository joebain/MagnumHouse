
using System;
using System.Net.Sockets;

namespace MagnumHouseLib
{
	public class NetworkClient
	{

		TcpClient client;
		
		public NetworkClient (string serverAddress)
		{
			client = new TcpClient(serverAddress, NetworkUtil.port);
			client.BeginConnect(serverAddress, NetworkUtil.port, new AsyncCallback(Connected), null);
		}
		
		public void Connected(IAsyncResult ar) {
			client.EndConnect(ar);
			
			Console.WriteLine("client connected");
		}
	}
}
