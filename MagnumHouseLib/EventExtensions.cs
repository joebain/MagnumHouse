
using System;

namespace MagnumHouseLib
{
	public static class EventExtensions
	{
		public static void Raise<T>(this Action<T> _event, T _arg) {
			var tmpEvent = _event;
			if (tmpEvent != null) tmpEvent(_arg);
		}
	}
}
