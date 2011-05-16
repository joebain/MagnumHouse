
using System;
namespace MagnumHouseLib
{
	[Serializable]
	public abstract class TriggerDescription {
		public bool fadeIn, fadeOut;
		public abstract Trigger MakeReal(Screen level);
	}
}
