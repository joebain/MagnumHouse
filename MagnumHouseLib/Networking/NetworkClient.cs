
using System;
using System.Net.Sockets;
using System.Net;

using MagnumHouse;

namespace MagnumHouseLib
{
	public class NetworkClient
	{

		TcpClient client;
		IPAddress remoteAddr;
		NetworkStream stream;
		const int bufferSize = 100;
		protected byte [] readBuffer = new byte[bufferSize];
		
		public NetworkClient (string serverAddress)
		{
			remoteAddr = IPAddress.Parse(serverAddress);
			client = new TcpClient();
			
		}
		
		public void Connect() {
			Console.WriteLine("trying to connect");	
			client.BeginConnect(remoteAddr, NetworkUtil.port, new AsyncCallback(Connected), null);
			
		}
		
		private void Connected(IAsyncResult ar) {
			client.EndConnect(ar);
			
			Console.WriteLine("client connected");			
			
			stream = client.GetStream();
			stream.BeginRead(readBuffer, 0, bufferSize, new AsyncCallback(GotMessage), null);
			
		}
		
		private void GotMessage(IAsyncResult ar) {
			Console.WriteLine("got a message: ");
			HandleMessage();
			stream.EndRead(ar);
		}
		
		protected virtual void HandleMessage() {
			if (GangsterMessage.SIs(readBuffer)) {
				var gm = new GangsterMessage();
				gm.FromBytes(readBuffer);
				Console.WriteLine("position is " + gm.Position);
			}
		}
		
		public void SendMessage(INetworkMessage _message) {
			stream.BeginWrite(_message.GetBytes(), 0, _message.Size, new AsyncCallback(SentMessage), null);
		}
		
		private void SentMessage (IAsyncResult ar) {
			Console.WriteLine("sent a message");
			stream.EndWrite(ar);
		}
	}
}
