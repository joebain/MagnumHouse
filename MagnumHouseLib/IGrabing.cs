
using System;

namespace MagnumHouseLib
{
	public interface IGrabing : IDeadable
	{
		void Grab(Action<Layer> _draw);
	}
}
