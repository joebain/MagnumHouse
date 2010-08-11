using System;

namespace MagnumHouseLib
{
	public interface IUpdateable : IDeadable
	{
		int Id {get;}
		void Update(float _delta);
	}
}
