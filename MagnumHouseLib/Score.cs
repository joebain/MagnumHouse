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
		float margin = 0.1f;
		
		public Layer Layer { get { return Layer.Normal; } }
		public Priority Priority {get { return Priority.Middle; } }
		
		public Score (Gangster _gangster)
		{
			m_gangster = _gangster;
			Size.Y = height;
		}
		
		public void Draw() {
			Gl.glPushMatrix();
			Gl.glTranslatef(Position.X, Position.Y, 1f);
			
			for (int block_i = 0 ; block_i <= Magnum.maxHealth ; block_i++) {
				DrawBlackBlock(block_i, 0.9f);
			}
			
			int i_health = (int)Math.Floor(m_health);
			for (int block_i = 0 ; block_i < i_health ; block_i ++) {
				DrawWhiteBlock(block_i, 0.9f);
			}
			float last_bit = m_health - i_health;
			DrawWhiteBlock(i_health, last_bit*0.9f);
			
			Gl.glPopMatrix();
		}
		
		private void DrawWhiteBlock(int block_i, float block_width) {
			Gl.glColor3f(1,1,1);
			Gl.glBegin(Gl.GL_TRIANGLE_STRIP);
				Gl.glVertex2f(block_i*width, 0f);
				Gl.glVertex2f((block_width + block_i)*width, 0f);
				Gl.glVertex2f(block_i*width, height);
				Gl.glVertex2f((block_width + block_i)*width, height);
			Gl.glEnd();
		}
		
		private void DrawBlackBlock(int block_i, float block_width) {
			Gl.glColor3f(1f,0.2f,0.2f);
			//top
			Gl.glBegin(Gl.GL_TRIANGLE_STRIP);
				Gl.glVertex2f(block_i*width-margin, -margin);
				Gl.glVertex2f((block_width + block_i)*width+margin, -margin);
				Gl.glVertex2f(block_i*width-margin, 0);
				Gl.glVertex2f((block_width + block_i)*width+margin, 0);
			Gl.glEnd();
			//bottom
			Gl.glBegin(Gl.GL_TRIANGLE_STRIP);
				Gl.glVertex2f(block_i*width-margin, height);
				Gl.glVertex2f((block_width + block_i)*width+margin, height);
				Gl.glVertex2f(block_i*width-margin, height+margin);
				Gl.glVertex2f((block_width + block_i)*width+margin, height+margin);
			Gl.glEnd();
			//left
			Gl.glBegin(Gl.GL_TRIANGLE_STRIP);
				Gl.glVertex2f(block_i*width-margin, -margin);
				Gl.glVertex2f(block_i*width, -margin);
				Gl.glVertex2f(block_i*width-margin, height+margin);
				Gl.glVertex2f(block_i*width, height+margin);
			Gl.glEnd();
			//right
			Gl.glBegin(Gl.GL_TRIANGLE_STRIP);
				Gl.glVertex2f((block_width + block_i)*width, -margin);
				Gl.glVertex2f((block_width + block_i)*width+margin, -margin);
				Gl.glVertex2f((block_width + block_i)*width, height+margin);
				Gl.glVertex2f((block_width + block_i)*width+margin, height+margin);
			Gl.glEnd();
		}
		
		public void Update(float _delta) {
			m_health = m_gangster.Magnum.health;
			Size.X = m_health * width;
		}
	}
}
