
using System;
using Tao.OpenGl;

namespace MagnumHouseLib
{

	public class Tile : IDrawable
	{
		public const int Size = 40;
		
		Vector2i m_position;
		
		
		public Tile (Vector2i _position)
		{
			m_position = _position;
		}
		
		public void Draw() {
			Gl.glPushMatrix();
			
			Gl.glTranslatef(m_position.X, m_position.Y, 0);
			Gl.glColor3f(0.5f,0.5f,0.5f);
			
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
