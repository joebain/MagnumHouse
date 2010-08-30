
using System;

using Tao.OpenGl;

namespace MagnumHouseLib
{


	public class Floor : IDrawable
	{
		
		public Layer Layer { get { return Layer.Normal; }}
		public Priority Priority { get { return Priority.Middle; }}

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
