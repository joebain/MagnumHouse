
using System;

namespace MagnumHouseLib
{
	[Serializable]
	public abstract class MessageContent
	{
		public Int32 Id;
		public virtual void ApplyTo(Object o) {}
	}
}
