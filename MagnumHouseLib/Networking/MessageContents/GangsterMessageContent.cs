
using System;

namespace MagnumHouseLib
{
	public class GangsterMessageContent : MessageContent
	{
		public Vector2f Position;
		
		public override void ApplyTo(Object o) {
			var gangster = o as Gangster;
			if (gangster != null) {
				gangster.Position = Position;
			}
		}
	}
}
