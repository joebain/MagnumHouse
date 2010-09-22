
using System;
namespace MagnumHouseLib
{
	[Serializable]
	public class TimedTriggerDescription : TriggerDescription {
		public float start, length;
		
		public override Trigger MakeReal(Screen level) {
			return new TimedTrigger(start, length);
		}
	}
}
