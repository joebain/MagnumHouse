
using System;

namespace MagnumHouseLib
{
	public interface IAnimator
	{
		float Step(float delta);
		float Position {get;}
	}
}
