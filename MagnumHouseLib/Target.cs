using System;
using Tao.OpenGl;

namespace MagnumHouse
{
	public class Target : Thing2D, IDrawable, IShootable
	{
		private float radius = 0.5f;
		Sound explodeSound = new Sound("sounds/explosion.wav");
		
		public Target ()
		{
			Size = new Vector2f(radius*2, radius*2);
		}
		
		public void Draw ()
		{
			Gl.glPushMatrix();
			Gl.glTranslatef(Position.X+radius, Position.Y+radius, 0);
			
			float outSize = radius;
			float inSize = radius/2;
			
			Gl.glBegin(Gl.GL_TRIANGLE_FAN);
			Gl.glColor3f(1,1,1);
			for (float i = 0 ; i < Math.PI*2 ; i += (float)Math.PI/4) {
				var v = new Vector2f((float)Math.Sin(i), (float)Math.Cos(i));
				v *= outSize;
				Gl.glVertex2f(v.X,v.Y);
			}
			Gl.glEnd();
			
			Gl.glBegin(Gl.GL_TRIANGLE_FAN);
			Gl.glColor3f(1,0,0);
			for (float i = 0 ; i < Math.PI*2 ; i += (float)Math.PI/4) {
				var v = new Vector2f((float)Math.Sin(i), (float)Math.Cos(i));
				v *= inSize;
				Gl.glVertex2f(v.X,v.Y);
			}
			Gl.glEnd();
			
			Gl.glPopMatrix();
		}
		
		public void GotShot(Slug _slug) {
			this.Dead = true;
			explodeSound.Play();
		}
	}
}
