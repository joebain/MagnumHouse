using System;

using MagnumHouse;

namespace MagnumHouseLib
{
	public class GangsterVisage
	{
		public Vector2f position = new Vector2f();
		
		public GangsterVisage ()
		{
		}
		
		public void Receive(byte[] buffer) {
			if (GangsterMessage.SIs(buffer)) {
				var message = new GangsterMessage(buffer);
				position = message.Position;
			}
		}
		
		public GangsterMessage Relay() {
			return new GangsterMessage(position);
		}
	}
}
