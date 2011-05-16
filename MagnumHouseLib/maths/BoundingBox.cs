using System;
using Tao.OpenGl;

namespace MagnumHouseLib
{
	[Serializable]
	public class BoundingBox
	{
		public float Top;
		public float Bottom;
		public float Left;
		public float Right;
		
		public BoundingBox ()
		{
		}
		
		public BoundingBox(Vector2f bottomLeft, Vector2f topRight) {
			Top = topRight.Y;
			Bottom = bottomLeft.Y;
			Left = bottomLeft.X;
			Right = topRight.X;
		}
		
		public BoundingBox(float _left, float _bottom, float _right, float _top) {
			Top = _top;
			Bottom = _bottom;
			Left = _left;
			Right = _right;
		}
		
		public float Width { get { return Right - Left;}}
		public float Height { get { return Top - Bottom;}}
		
		public Vector2f TopLeft {get{return new Vector2f(Left, Top);}}
		public Vector2f BottomRight {get{return new Vector2f(Right, Bottom);}}
		public Vector2f Centre {get{return new Vector2f((Right + Left)/2, (Top + Bottom)/2);}}
		public Vector2f Size {get{return new Vector2f(Width, Height);}}
		
		public bool Overlaps(BoundingBox _other) {
			
			bool yIntersect = 
				Top < _other.Top && Top > _other.Bottom ||
				_other.Top < Top && _other.Top > Bottom;
			bool xIntersect =
				Left < _other.Right && Left > _other.Left ||
				_other.Left < Right && _other.Left > Left;
			
			return yIntersect && xIntersect;
		}
		
		public bool Contains(Vector2f _point) {
			return _point.X > Left && _point.X < Right &&
				_point.Y > Bottom && _point.Y < Top;
		}
		
		public override string ToString ()
		{
			return string.Format("[T:{0}, B:{1}, L:{2}, R:{3}]", Top, Bottom, Left, Right);
		}
		
		public void Draw(Colour colour) {
			Gl.glPushMatrix();
			Gl.glColor4fv(colour.Array);
			Gl.glBegin(Gl.GL_TRIANGLE_FAN);
			Gl.glVertex2f(Left, Bottom);
			Gl.glVertex2f(Right, Bottom);
			Gl.glVertex2f(Right, Top);
			Gl.glVertex2f(Left, Top);
			Gl.glEnd();
			Gl.glPopMatrix();
		}
		
		public BoundingBox Clone {get{return new BoundingBox(Left, Bottom, Right, Top);}}
	}
}
