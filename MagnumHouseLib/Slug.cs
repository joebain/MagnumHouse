
using System;
using Tao.OpenGl;

namespace MagnumHouseLib
{
	public enum ThingsToHit {
		Wall,
		Gangster,
		Phony,
		OOB //out of bounds
	}

	public class Slug : Thing2D, IUpdateable, IDrawable
	{
		public Layer Layer { get { return Layer.Normal; }}
		
		public override Vector2f Size { get; set; }
		
		public override Vector2f Position {get; set;}
		public Vector2f Direction;
		
		public float speed = 20.0f;
		public override Vector2f Speed {
			get { return new Vector2f(speed); }
			set { speed = value.Length(); }
		}
		
		private Magnum m_magnum;
		public Magnum Magnum{get { return m_magnum;}}
		
		public Slug (Magnum _magnum)
		{
			m_magnum = _magnum;
			Size = new Vector2f(m_magnum.Size)/2f;
		}
		
		public void Update(float _delta) {
			Position += Direction * speed * _delta;
		}
		
		public void Draw() {
			Gl.glPushMatrix();
			
			Gl.glColor3f(0.742f,0.523f,0f);
			
			Gl.glTranslatef(Position.X, Position.Y, 0);
			Gl.glBegin(Gl.GL_TRIANGLE_FAN);
			Gl.glVertex2f(0,0);
			for (int i = 0 ; i < 13 ; i ++) {
				double r = (i/12.0)*Math.PI*2;
				double x = Math.Sin(r);
				double y = Math.Cos(r);
				Gl.glVertex2f((float)x*Size.X,(float)y*Size.Y);
			}
			
			Gl.glEnd();
			Gl.glPopMatrix();
		}
		
		public void HitSomething(ThingsToHit _hit, Object _thing) {
			Dead = true;
			if (_hit == ThingsToHit.Gangster)
				m_magnum.MadeAKill((IShootable)_thing);
		}
		
		
	}
}
