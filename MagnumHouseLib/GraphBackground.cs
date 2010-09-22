using Tao.OpenGl;
using System;

namespace MagnumHouseLib
{
	public class GraphBackground : IDrawable
	{

		public Priority Priority {get { return Priority.Back; } }
		public Layer Layer { get;set; }
		
		Vector2i m_size;
		
		bool m_dead = false;
		public bool Dead{get{return m_dead;}}
		public void Die(){m_dead = true;}
		
		public GraphBackground(Vector2i _size) {
			Layer = Layer.Blurry;
			m_size = _size;
		}
		
		public void Draw() {
			Gl.glColor3f(0.0f,0.0f,0.0f);
			Gl.glBegin(Gl.GL_TRIANGLE_STRIP);
			Gl.glVertex2f(0,0);
			Gl.glVertex2f(0, m_size.Y);
			Gl.glVertex2f(m_size.X, 0);
			Gl.glVertex2f(m_size.X, m_size.Y);
			Gl.glEnd();
			
			Gl.glColor3f(1f,0f, 0f);
			Gl.glBegin(Gl.GL_LINES);
			for (int x = 0 ; x <= m_size.X; ++x) {
				Gl.glVertex2f(x, 0);
				Gl.glVertex2f(x, m_size.Y);
			}
			for (int y = 0 ; y <= m_size.Y ; ++y) {
				Gl.glVertex2f(0, y);
				Gl.glVertex2f(m_size.X, y);
			}
			Gl.glEnd();
		}
	}
}
