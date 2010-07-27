using System;

namespace MagnumHouseLib
{
	public interface IUpdateable : IDeadable
	{
		void Update(float _delta);
	}
}
