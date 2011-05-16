
using System;
namespace MagnumHouseLib
{
	[Serializable]
	public class FadeEventDescription : EventDescription {
		public Colour startColour;
		public Colour endColour;
		public float duration;
		
		public override Event MakeReal(Screen level) {
			FadeEvent fEvent = new FadeEvent(
				level.Game.Camera, startColour, endColour,
			    duration, trigger.MakeReal(level), level.House);
			return fEvent;
		}
	}
}
