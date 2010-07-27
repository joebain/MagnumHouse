using System;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using System.Linq;

using MagnumHouseLib;

namespace MagnumHouseLib
{
	public class NetworkServer : IUpdateable
	{
		TcpListener listener;
		
		List<Visage> visages = new List<Visage>();
		List<Visage> disconnectedClients = new List<Visage>();
		WorldVisage world = new WorldVisage();
		public WorldVisage World { get { return world; } }
		static int maxClients = 4;
		int idcount = 1;
		
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
				Int32 id = idcount++;
				g.id = id;
				world.AddGangster(g);
				SendHello(visage, id);
				AcceptClient();
			} else {
				//too many clients
			}
		}
		
		public void SendHello(Visage visage, Int32 id) {
			
			var message = new HelloMessage(id);
			
			Console.WriteLine("sending hello");
			Console.WriteLine("can write? " + visage.client.GetStream().CanWrite);
			visage.client.GetStream().BeginWrite(message.GetBytes(), 0, message.Size, new AsyncCallback(SentHello), visage);
		}
		
		public void ReadMessage(Visage _visage) {
			Console.WriteLine("waiting for messages");
			Array.Clear(_visage.readBuffer,0,Visage.bufferSize);
			_visage.client.GetStream().BeginRead(_visage.readBuffer, 0, Visage.bufferSize, new AsyncCallback(GotMessage), _visage);
		}
		
		private void ClientDisconnected(Visage visage) {
			disconnectedClients.Add(visage);
		}
		
		public void GotMessage(IAsyncResult ar) {
			Console.WriteLine("got a message");
			
			var visage = (Visage)ar.AsyncState;
			visage.client.GetStream().EndRead(ar);
			
			Console.Write("== ");
			for (int i = 0 ; i < 10 ; i++) {
				Console.Write(visage.readBuffer[i]);
			}
			Console.WriteLine(" ==");
			
			visage.gangster.Receive(visage.readBuffer);
		
			if (GoodbyeMessage.SIs(visage.readBuffer) || visage.readBuffer[0] == 0) {
				ClientDisconnected(visage);
				Console.WriteLine("client said goodbye");
			} else {
				ReadMessage(visage);
			}
		}
		
		public void Update(float _delta) {
			foreach (var visage in visages) {
				foreach (var other in visages.Where(v => v.gangster.id != visage.gangster.id)) {
					var message = other.gangster.Relay();
					visage.client.GetStream().BeginWrite(message.GetBytes(), 0, message.Size, new AsyncCallback(SentMessage), visage);
				}
			}
			if (disconnectedClients.Any()) {
				foreach (var visage in disconnectedClients) {
					visages.Remove(visage);
					world.RemoveGangster(visage.gangster);
				}
				disconnectedClients.Clear();
			}
		}
		
		public void SentMessage(IAsyncResult ar) {
			Console.WriteLine("sent a message");
			var visage = (Visage)ar.AsyncState;
			visage.client.GetStream().EndWrite(ar);
		}
		
		public void SentHello(IAsyncResult ar) {
			Console.WriteLine("sent a hello");
			var visage = (Visage)ar.AsyncState;
			visage.client.GetStream().EndWrite(ar);
			
			ReadMessage(visage);
		}
		
		public bool Dead { get { return false; } }
		
		public void Die() {}
	}
}
