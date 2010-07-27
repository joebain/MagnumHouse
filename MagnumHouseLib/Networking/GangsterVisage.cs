using System;

using MagnumHouseLib;

namespace MagnumHouseLib
{
	public class GangsterVisage
	{
		public Vector2f position = new Vector2f();
		public Int32 id;
		public bool sayingGoodbye;
		
		public GangsterVisage ()
		{
		}
		
		public void Receive(byte[] buffer) {
			if (GangsterMessage.SIs(buffer)) {
				Receive(new GangsterMessage(buffer));
			} else if (GoodbyeMessage.SIs(buffer)) {
				Receive(new GoodbyeMessage(buffer));
			}
		}
		
		public void Receive(GangsterMessage _message) {
			position = _message.Position;
			id = _message.Id;
		}
		
		public void Receive(GoodbyeMessage _message) {
			id = _message.Id;
			sayingGoodbye = true;
		}
		
		public INetworkMessage Relay() {
			if (sayingGoodbye) {
				Console.WriteLine("relaying goodbye, id {0}",id);
				return new GoodbyeMessage(id);
			} else {
				Console.WriteLine("relaying pos, id {0}",id);
				return new GangsterMessage(id, position);
			}
		}
	}
}
