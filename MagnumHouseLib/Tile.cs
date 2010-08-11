
using System;
using Tao.OpenGl;

namespace MagnumHouseLib
{

	public class Tile : IDrawable
	{
		public Layer Layer { get { return Layer.Normal; }}
		
		public const int Size = 40;
		
		Vector2i m_position;
		
		
		public Tile (Vector2i _position)
		{
			m_position = _position;
		}
		
		public void Draw() {
			Gl.glPushMatrix();
			
			Gl.glTranslatef(m_position.X, m_position.Y, 0);
			if (m_position.Y <= 0) {
				Gl.glColor3f(0.449f,0.797f,0.301f); //nice green grass
			} else if (m_position.Y <= 12) {
				Gl.glColor3f(0.797f,0.570f,0.301f); //brown trees
			} else {
				Gl.glColor3f(0.684f,0.852f,0.859f); //blue sky
			}
			
			Gl.glBegin(Gl.GL_TRIANGLE_STRIP);
			
			Gl.glVertex2f(0, 0);
			Gl.glVertex2f(0.9f, 0);
			Gl.glVertex2f(0, 0.9f);
			Gl.glVertex2f(0.9f, 0.9f);
			
			Gl.glEnd();
			Gl.glPopMatrix();
		}
		
		public bool Dead {get { return false;}}
		
		public void Die() {}
	}
}
