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
		
		List<ClientDetails> clientDetails = new List<ClientDetails>();
		List<ClientDetails> disconnectedClients = new List<ClientDetails>();
		WorldVisage world = new WorldVisage();
		public WorldVisage World { get { return world; } }
		static int maxClients = 4;
		int idcount = 1;
		
		public int Id { get; set;}
		
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
			
			if (clientDetails.Count < maxClients) {
				Console.WriteLine("client connected");
				
				var visage = new ClientDetails() {client = client};
				clientDetails.Add(visage);
				Int32 id = idcount++;
				
				SendHello(visage, id);
				AcceptClient();
			} else {
				//too many clients
			}
		}
		
		public void SendHello(ClientDetails visage, Int32 id) {
			
			var message = new ClientHelloMessage(id);
			
			Console.WriteLine("sending hello");
			visage.client.GetStream().BeginWrite(message.GetBytes(), 0, message.Size, new AsyncCallback(SentHello), visage);
		}
		
		public void ReceiveMessage(ClientDetails details) {
			Console.WriteLine("waiting for messages");
			Array.Clear(details.readBuffer,0,ClientDetails.bufferSize);
			details.client.GetStream().BeginRead(details.readBuffer, 0, ClientDetails.bufferSize, new AsyncCallback(GotMessage), details);
		}
		
		private void ClientDisconnected(ClientDetails visage) {
			disconnectedClients.Add(visage);
		}
		
		public void GotMessage(IAsyncResult ar) {
			Console.WriteLine("got a message");
			
			var details = (ClientDetails)ar.AsyncState;
			details.client.GetStream().EndRead(ar);
			
			Console.Write("== ");
			for (int i = 0 ; i < 10 ; i++) {
				Console.Write(details.readBuffer[i]);
			}
			Console.WriteLine(" ==");
			
			var message = new GenericMessage(details.readBuffer);
			
			if (message.ContentType == typeof(GoodbyeMessageContent)) {
				ClientDisconnected(details);
				Console.WriteLine("client said goodbye");
			} else {
				HandleMessage(message);
				ReceiveMessage(details);
			}
		}
		
		protected virtual void HandleMessage(GenericMessage message) {
			
		}
		
		public void Update(float _delta) {
//			foreach (var details in clientDetails) {
//				foreach (var other in clientDetails.Where(v => v.gangster.id != details.gangster.id)) {
//					var message = other.gangster.Relay();
//					details.client.GetStream().BeginWrite(message.GetBytes(), 0, message.Size, new AsyncCallback(SentMessage), details);
//				}
//			}
//			if (disconnectedClients.Any()) {
//				foreach (var visage in disconnectedClients) {
//					clientDetails.Remove(visage);
//					world.RemoveGangster(visage.gangster);
//				}
//				disconnectedClients.Clear();
//			}
		}
		
		public void SentMessage(IAsyncResult ar) {
			Console.WriteLine("sent a message");
			var visage = (ClientDetails)ar.AsyncState;
			visage.client.GetStream().EndWrite(ar);
		}
		
		public void SentHello(IAsyncResult ar) {
			Console.WriteLine("sent a hello");
			var visage = (ClientDetails)ar.AsyncState;
			visage.client.GetStream().EndWrite(ar);
			
			ReceiveMessage(visage);
		}
		
		public bool Dead { get { return false; } }
		
		public void Die() {}
	}
}
