
using System;
using System.Collections.Generic;
using MagnumHouse;
using Tao.OpenGl;

namespace MagnumHouseLib
{
	public class GhostGangster : Thing2D, IUpdateable, IDrawable
	{
		GangsterVisage visage;
		
		public GhostGangster (GangsterVisage _visage)
		{
			visage = _visage;
		}
		
		public void Update (float _delta)
		{
			Position = visage.position;
		}
		
		public void Draw() {
			Gl.glPushMatrix();
			
				Gl.glTranslatef(Position.X, Position.Y, 0);
				
				Gl.glColor3f(0.5f,0.3f,0.1f);
				
				Gl.glBegin(Gl.GL_TRIANGLE_FAN);
				
					Gl.glVertex2f(Position.X-0.5f, Position.Y);
					Gl.glVertex2f(Position.X, Position.Y + 1);
					Gl.glVertex2f(Position.X+0.5f, Position.Y);
			
				Gl.glEnd();
			
			Gl.glPopMatrix();
		}
	}
}
