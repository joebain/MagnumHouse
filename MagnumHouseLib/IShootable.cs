
using System;

namespace MagnumHouse
{
	public interface IShootable : IThing2D, IDeadable
	{
		void GotShot(Slug _slug);
	}
}
