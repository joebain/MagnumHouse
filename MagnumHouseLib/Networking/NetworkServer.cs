using System;
using System.Net.Sockets;
using System.Net;

namespace MagnumHouseLib
{
	public class NetworkServer
	{
		TcpListener listener;
		
		public NetworkServer ()
		{
			IPAddress localAddr = IPAddress.Parse("127.0.0.1");

			listener = new TcpListener(localAddr, NetworkUtil.port);
			listener.Start();
			listener.BeginAcceptTcpClient(new AsyncCallback(AcceptConnection), null);
		}
		
		public void AcceptConnection(IAsyncResult ar) {
			listener.EndAcceptTcpClient(ar);
			
			Console.WriteLine("server connected");
		}
	}
}
