
using System;

namespace MagnumHouseLib
{


	public class Vector2i
	{
		public int X;
		public int Y;
		
		public Vector2i ()
		{
		}
		
		public Vector2i (int _x, int _y)
		{
			X = _x;
			Y = _y;
		}
		
		public Vector2i Clone () {
			return new Vector2i(X,Y);
		}
		
//		public Vector2f ScreenToGameCoords() {
//			return new Vector2f ((float)X / Tile.Size, (float)(Game.ScreenHeight - Y) / Tile.Size);
//		}	
		
		public override string ToString ()
		{
			return string.Format("(X: {0}, Y: {1})",X,Y);
		}
		
		public Vector2f ToF() {
			return new Vector2f(X, Y);
		}
		
		public static Vector2i operator - (Vector2i _lhs, Vector2i _rhs) {
			return new Vector2i(_lhs.X - _rhs.X, _lhs.Y - _rhs.Y);
		}

	}
}
