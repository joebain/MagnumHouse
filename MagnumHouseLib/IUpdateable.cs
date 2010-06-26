using System;

namespace MagnumHouse
{
	public interface IUpdateable : IDeadable
	{
		void Update(float _delta);
	}
}
