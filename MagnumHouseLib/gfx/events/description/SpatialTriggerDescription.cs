
using System;
namespace MagnumHouseLib
{
	[Serializable]
	public class SpatialTriggerDescription : TriggerDescription {
		public BoundingBox boxOn, boxOff;
		public bool useBoxOff;
		
		public override Trigger MakeReal(Screen level) {
			return new SpatialTrigger(boxOn, boxOff, useBoxOff, level.Character);
		}
	}
}
