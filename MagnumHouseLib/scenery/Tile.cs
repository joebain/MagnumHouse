
using System;
using Tao.OpenGl;

namespace MagnumHouseLib
{

	public class Tile : IDrawable
	{
		public Layer Layer { get { return Layer.Normal; }}
		public Priority Priority { get { return Priority.Middle; }}
		
		public const int Size = 20;
		
		Vector2i m_position;
		
		
		public Tile (Vector2i _position)
		{
			m_position = _position;
		}
		
		public void Draw() {
			Draw(m_position);
		}
		
		public static void Draw(Vector2i pos) {
			Draw(pos, 0, 0);
		}
		
		public static void Draw(Vector2i pos, float depth1, float depth2) {
			Gl.glPushMatrix();
			
			Gl.glTranslatef(pos.X, pos.Y, 0);
			if (pos.Y <= 0) {
				Gl.glColor3f(0.449f,0.797f,0.301f); //nice green grass
			} else if (pos.Y <= 12) {
				Gl.glColor3f(0.797f,0.570f,0.301f); //brown trees
			} else {
				Gl.glColor3f(0.684f,0.852f,0.859f); //blue sky
			}
			
			Gl.glBegin(Gl.GL_TRIANGLE_STRIP);
			
			Gl.glVertex3f(0, 0, depth1);
			Gl.glVertex3f(1f, 0, 0);
			Gl.glVertex3f(0, 1f, 0);
			Gl.glVertex3f(1f, 1f, depth2);
			
			Gl.glEnd();
			Gl.glPopMatrix();
		}
		
		public bool Dead {get { return false;}}
		
		public void Die() {}
	}
}
