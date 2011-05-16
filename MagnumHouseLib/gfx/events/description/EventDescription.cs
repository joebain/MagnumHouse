
using System;

namespace MagnumHouseLib
{
	[Serializable]
	public abstract class EventDescription
	{
		public TriggerDescription trigger;
		
		public abstract Event MakeReal(Screen level);
	}
}
