
using System;

namespace MagnumHouseLib
{
	[Serializable]
	public class Vector2f
	{

		public float X;
		public float Y;
		
		public static Vector2f Zero {get { return new Vector2f(); } }
		public static Vector2f Unit {get { return new Vector2f(1); } }
		
		public Vector2f ()
		{
		}
		
		public Vector2f(float _val) {
			X = _val;
			Y = _val;
		}
		
		public Vector2f (float _x, float _y)
		{
			X = _x;
			Y = _y;
		}
		
		public void Cap (float _max) {
			if (LengthSquared() > _max*_max) {
				Normalise();
				Multiply(_max);
			}
		}
		
		public void Cap (Vector2f max) {
			if (Math.Abs(X) > max.X)
				X = max.X * Math.Sign(X);
			if (Math.Abs(Y) > max.Y)
				Y = max.Y * Math.Sign(Y);
		}
		
		public void Ensure (float _min) {
			if (LengthSquared() < _min * _min) {
				Normalise();
				Multiply(_min);
			}
		}
		
		public void Ensure (Vector2f min) {
			if (Math.Abs(X) > min.X)
				X = min.X * Math.Sign(X);
			if (Math.Abs(Y) > min.Y)
				Y = min.Y * Math.Sign(Y);
		}
		
		public void Clamp(Vector2f min, Vector2f max) {
			if (X < min.X) X = min.X;
			else if (X > max.X) X = max.X;
			if (Y < min.Y) Y = min.Y;
			else if (Y > max.Y) Y = max.Y;
		}
		
		public void Normalise() {
			float length = Length();
			Divide(length);
		}
		
		public float Length() {
			return (float)Math.Sqrt(LengthSquared());
		}
		
		public float LengthSquared() {
			return X*X + Y*Y;
		}
		
		public void Multiply(float _m) {
			X *= _m;
			Y *= _m;
		}
		
		public void Divide (float _d) {
			X /= _d;
			Y /= _d;
		}
		
		public float Angle() {
			return (float)Math.Atan2(X,Y);
		}
		
		public static Vector2f operator + (Vector2f _lhs, Vector2f _rhs) {
			return new Vector2f(_lhs.X + _rhs.X, _lhs.Y + _rhs.Y);
		}
		
		public static Vector2f operator - (Vector2f _lhs, Vector2f _rhs) {
			return new Vector2f(_lhs.X - _rhs.X, _lhs.Y - _rhs.Y);
		}
		
		public static Vector2f operator - (Vector2f _rhs) {
			return new Vector2f(-_rhs.X, -_rhs.Y);
		}
		
		public static Vector2f operator * (Vector2f _lhs, float _rhs) {
			return new Vector2f(_lhs.X * _rhs, _lhs.Y * _rhs);
		}
		
		public static Vector2f operator * (Vector2f _lhs, Vector2f _rhs) {
			return new Vector2f(_lhs.X * _rhs.X, _lhs.Y * _rhs.Y);
		}
		
		public static Vector2f operator / (Vector2f _lhs, float _rhs) {
			return new Vector2f(_lhs.X / _rhs, _lhs.Y / _rhs);
		}
		
		public static Vector2f operator / (Vector2f _lhs, Vector2f _rhs) {
			return new Vector2f(_lhs.X / _rhs.X, _lhs.Y / _rhs.Y);
		}
		
		public override string ToString ()
		{
			return string.Format("(X: {0}, Y: {1})", X, Y);
		}
		
		public string CSV() {
			return string.Format("{0}, {1}", X , Y);
		}
		
		public Vector2i Round() {
			return new Vector2i((int)Math.Round(X), (int)Math.Round(Y));
		}
		
		public Vector2f Snap(Vector2f snap) {
			return (this / snap).Round().ToF() * snap;
		}
		
		public Vector2i Ceiling() {
			return new Vector2i((int)Math.Ceiling(X), (int)Math.Ceiling(Y));
		}
		
		public Vector2i Floor() {
			return new Vector2i((int)Math.Floor(X), (int)Math.Floor(Y));
		}
		
		public static Vector2f Right { get { return new Vector2f(1,0);}}
		public static Vector2f Left { get { return new Vector2f(-1,0);}}
		public static Vector2f Up { get { return new Vector2f(0,1);}}
		public static Vector2f Down { get { return new Vector2f(0,-1);}}
		
		public bool Equals(Vector2f _other) {
			return X == _other.X && Y == _other.Y;
		}
		
		public static Vector2f Random(float min, float max) {
			System.Random random = new System.Random(DateTime.Now.Millisecond);
			return new Vector2f((float)random.NextDouble() * (max-min) + min, (float)random.NextDouble() * (max-min) + min);
		}
		
		public Vector2f Clone() {
			return new Vector2f(X, Y);
		}
		
		public void Clamp(BoundingBox bounds) {
			if (X > bounds.Right) X = bounds.Right;
			else if (X < bounds.Left) X = bounds.Left;
			
			if (Y < bounds.Bottom) Y = bounds.Bottom;
			else if (Y > bounds.Top) Y = bounds.Top;
		}
	}
}
