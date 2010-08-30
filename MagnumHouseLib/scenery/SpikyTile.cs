
using System;
using Tao.OpenGl;

namespace MagnumHouseLib
{
	public class SpikyTile : IDrawable
	{
		Vector2i m_position;

		public Layer Layer { get { return Layer.Normal; }}
		public Priority Priority { get { return Priority.Middle; }}
		
		public SpikyTile(Vector2i position) {
			m_position = position;
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
			
			Gl.glVertex2f(0, 0.0f); //bottom left
			Gl.glVertex2f(0.9f, 0.0f); //bottom right
			
			Gl.glVertex2f(0, 0.5f); //top left
			Gl.glVertex2f(0.9f, 0.5f); //top right
			
			Gl.glEnd();
			
			Gl.glBegin(Gl.GL_TRIANGLES);
			
			for (int i = 0 ; i < 3 ; i++) {
				float offset = i * 0.3f;
				Gl.glVertex2f(0 + offset, 0.5f);
				Gl.glVertex2f(0.15f + offset, 0.9f);
				Gl.glVertex2f(0.3f + offset, 0.5f);
			}
			
			Gl.glEnd();
			Gl.glPopMatrix();
		}
		
		public bool Dead {get { return false;}}
		
		public void Die() {}
		
	}
}
