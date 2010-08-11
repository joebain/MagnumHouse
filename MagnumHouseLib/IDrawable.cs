using System;

namespace MagnumHouseLib
{
	public enum Layer {
		Pixelly, Normal, Blurry
	}
	public interface IDrawable : IDeadable
	{
		void Draw();
		Layer Layer {get;}
	}
}
