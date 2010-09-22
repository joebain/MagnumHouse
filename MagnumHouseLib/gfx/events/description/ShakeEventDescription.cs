
using System;

namespace MagnumHouseLib
{
	[Serializable]
	public class ShakeEventDescription : EventDescription {
		public float shakiness;
		
		public override Event MakeReal(Screen level) {
			ShakeEvent fEvent = new ShakeEvent(
				level.Game.Camera, shakiness, trigger.MakeReal(level), level.House);
			return fEvent;
		}
	}
}
