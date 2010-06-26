
using System;

namespace MagnumHouse
{


	public class Vector2f
	{

		public float X;
		public float Y;
		
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
		
		public void Ensure (float _min) {
			if (LengthSquared() < _min * _min) {
				Normalise();
				Multiply(_min);
			}
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
		
		public static Vector2f operator * (Vector2f _lhs, float _rhs) {
			return new Vector2f(_lhs.X * _rhs, _lhs.Y * _rhs);
		}
		
		public static Vector2f operator / (Vector2f _lhs, float _rhs) {
			return new Vector2f(_lhs.X / _rhs, _lhs.Y / _rhs);
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
	}
}
