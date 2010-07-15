using System;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using System.Linq;

using MagnumHouse;

namespace MagnumHouseLib
{
	public class NetworkServer : IUpdateable
	{
		TcpListener listener;
		
		List<Visage> visages = new List<Visage>();
		WorldVisage world = new WorldVisage();
		public WorldVisage World { get { return world; } }
		static int maxClients = 4;
		
		public NetworkServer ()
		{
			IPAddress localAddr = IPAddress.Parse("127.0.0.1");

			listener = new TcpListener(localAddr, NetworkUtil.port);
			
		}
		
		public void Start() {
			listener.Start();
			AcceptClient();
		}
		
		private void AcceptClient() {
			listener.BeginAcceptTcpClient(new AsyncCallback(AcceptConnection), listener);
			Console.WriteLine("accepting connections");
		}
		
		private void AcceptConnection(IAsyncResult ar) {
			
			var client = listener.EndAcceptTcpClient(ar);
			
			if (visages.Count < maxClients) {
				Console.WriteLine("client connected");
				
				var g = new GangsterVisage();
				var visage = new Visage() {gangster = g, client = client};
				visages.Add(visage);
				world.AddGangster(g);
				
				ReadMessage(visage);
				AcceptClient();
			} else {
				//too many clients
			}
		}
		
		public void SentMessage(IAsyncResult ar) {
			Console.WriteLine("sent a message to the client");
			((NetworkStream)ar.AsyncState).EndWrite(ar);
		}
		
		public void ReadMessage(Visage _visage) {
			Console.WriteLine("waiting for messages");
			Array.Clear(_visage.readBuffer,0,Visage.bufferSize);
			_visage.client.GetStream().BeginRead(_visage.readBuffer, 0, Visage.bufferSize, new AsyncCallback(GotMessage), _visage);
		}
		
		public void GotMessage(IAsyncResult ar) {
			Console.WriteLine("got a message");
			
			var visage = (Visage)ar.AsyncState;
			
			if(visage.readBuffer[0] == 0) {
				visage.client.GetStream().EndRead(ar);
				visages.Remove(visage);
				Console.WriteLine("client has disconnected");
				return;
			} else {
				Console.WriteLine("some other kind of message");
				visage.gangster.Receive(visage.readBuffer);
			}
			Console.Write("== ");
			for (int i = 0 ; i < 10 ; i++) {
				Console.Write(visage.readBuffer[i]);
			}
			Console.WriteLine(" ==");
			
			
			visage.client.GetStream().EndRead(ar);
			ReadMessage(visage);
		}
		
		public void Update(float _delta) {
			foreach (var visage in visages) {
				foreach (var other in visages.Where(v => v != visage)) {
					var message = other.gangster.Relay();
					visage.client.GetStream().BeginWrite(message.GetBytes(), 0, message.Size, new AsyncCallback(SentMessage), visage);
				}
			}
		}
		
		public void SentMessage(IAsyncResult ar) {
			Console.WriteLine("sent a message");
			var visage = (Visage)ar.AsyncState;
			visage.client.GetStream().EndWrite(ar);
		}
	}
}
