using System;

namespace MagnumHouseLib
{
	//[Flags]
	public enum Layer {
		Pixelly = 1<<0, Normal = 1<<1, Blurry = 1<<2, FX = 1<<3, All = ~0
	}
	
	public enum Priority {
		Back, Middle, Front
	}
	
	public class DrawableBack{}
	public class DrawableMiddle{}
	public class DrawableFront{}
	
	public interface IDrawable : IDeadable
	{
		void Draw();
		Layer Layer {get;}
		Priority Priority {get;}
	}
}
