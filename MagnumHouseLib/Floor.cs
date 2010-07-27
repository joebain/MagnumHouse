
using System;

using Tao.OpenGl;

namespace MagnumHouseLib
{


	public class Floor : IDrawable
	{

		public Floor ()
		{
		}
		
		public void Draw() {
			Gl.glColor3f(0.5f,0.5f,0.5f);
			
			Gl.glBegin(Gl.GL_TRIANGLE_STRIP);
				Gl.glVertex2f(-1,0);
				Gl.glVertex2f(-1,-1);
				Gl.glVertex2f(1,0);
				Gl.glVertex2f(1,-1);
			Gl.glEnd();
		}
		
		public bool Dead {get { return false;}}
		
		public void Die() {}
	}
}
