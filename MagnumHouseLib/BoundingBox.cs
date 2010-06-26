using System;

namespace MagnumHouse
{
	public class BoundingBox
	{
		public float Top;
		public float Bottom;
		public float Left;
		public float Right;
		
		public BoundingBox ()
		{
		}
		
		public BoundingBox(float _left, float _bottom, float _right, float _top) {
			Top = _top;
			Bottom = _bottom;
			Left = _left;
			Right = _right;
		}
		
		public float Width { get { return Right - Left;}}
		public float Height { get { return Top - Bottom;}}
		
		public bool Overlaps(BoundingBox _other) {
			
			bool yIntersect = 
				Top < _other.Top && Top > _other.Bottom ||
				_other.Top < Top && _other.Top > Bottom;
			bool xIntersect =
				Left < _other.Right && Left > _other.Left ||
				_other.Left < Right && _other.Left > Left;
			
			return yIntersect && xIntersect;
		}
	}
}
