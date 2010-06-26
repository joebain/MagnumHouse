
using System;
using System.Net.Sockets;
using System.Net;

namespace MagnumHouseLib
{
	public class NetworkClient
	{

		TcpClient client;
		IPAddress remoteAddr;
		
		public NetworkClient (string serverAddress)
		{
			remoteAddr = IPAddress.Parse(serverAddress);
			client = new TcpClient();
			
		}
		
		public void Connect() {
			client.BeginConnect(remoteAddr, NetworkUtil.port, new AsyncCallback(Connected), null);
			Console.WriteLine("trying to connect");	
		}
		
		public void Connected(IAsyncResult ar) {
			client.EndConnect(ar);
			
			Console.WriteLine("client connected");
		}
	}
}
