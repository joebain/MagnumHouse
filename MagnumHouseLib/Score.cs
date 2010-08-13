using Tao.OpenGl;
using System;

namespace MagnumHouseLib
{
	public class Score : Thing2D, IDrawable, IUpdateable
	{
		Gangster m_gangster;
		float m_health;
		float width = 0.5f;
		float height = 1.0f;
		
		public Layer Layer { get { return Layer.Normal; } }
		
		public Score (Gangster _gangster)
		{
			m_gangster = _gangster;
			Size.Y = height;
		}
		
		public void Draw() {
			Gl.glPushMatrix();
			Gl.glTranslatef(Position.X, Position.Y, 1f);
			
			Gl.glColor3f(1f,1f,1f);
			int i_health = (int)Math.Floor(m_health);
			for (int block_i = 0 ; block_i < i_health ; block_i ++) {
				DrawBlock(block_i, 0.9f);
			}
			float last_bit = m_health - i_health;
			DrawBlock(i_health, last_bit*0.9f);
			
			Gl.glPopMatrix();
		}
		
		private void DrawBlock(int block_i, float block_width) {
			Gl.glBegin(Gl.GL_TRIANGLE_STRIP);
				Gl.glVertex2f(block_i*width, 0f);
				Gl.glVertex2f((block_width + block_i)*width, 0f);
				Gl.glVertex2f(block_i*width, height);
				Gl.glVertex2f((block_width + block_i)*width, height);
			Gl.glEnd();
		}
		
		public void Update(float _delta) {
			m_health = m_gangster.Magnum.health;
			Size.X = m_health * width;
		}
	}
}
