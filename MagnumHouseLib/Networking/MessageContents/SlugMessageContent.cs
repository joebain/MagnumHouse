using System;

namespace MagnumHouseLib
{
	public class SlugMessageContent : MessageContent
	{
		public Vector2f Position;
		public Vector2f Velocity;
		public Int32 GangsterId;
		
		public override void ApplyTo(Object o) {
			var slug = o as Slug;
			if (slug != null) {
				slug.Position = Position;
				slug.Direction = Velocity;
			}
		}
	}
}
