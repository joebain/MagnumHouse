using System;

namespace MagnumHouseLib
{
	public enum Layer {
		Pixelly, Normal, Blurry
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
