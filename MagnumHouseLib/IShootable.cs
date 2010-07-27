
using System;

namespace MagnumHouseLib
{
	public interface IShootable : IThing2D, IDeadable
	{
		void GotShot(Slug _slug);
	}
}
