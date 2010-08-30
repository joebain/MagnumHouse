
using System;
using System.Collections.Generic;
using MagnumHouseLib;
using Tao.OpenGl;

namespace MagnumHouseLib
{
	public class GhostGangster : Thing2D, IUpdateable, IDrawable
	{
		public Layer Layer { get { return Layer.Normal; }}
		public Priority Priority {get { return Priority.Middle; } }
		
		GangsterVisage visage;
		
		private float size = 0.6f;
		
		public GhostGangster (GangsterVisage _visage)
		{
			visage = _visage;
		}
		
		public override int Id {
			get {return visage.id;}
		}
		
		public void Update (float _delta)
		{
			Position = visage.position;
		}
		
		public void Draw() {
			Gl.glPushMatrix();
			Gl.glTranslatef(Position.X, Position.Y, 0);
			
			Gl.glColor3f(1, 0, 1);
                
            Gl.glBegin(Gl.GL_TRIANGLES);
                Gl.glVertex3f( 0.0f, 0.0f, 0.0f);
                Gl.glVertex3f( size, 0.0f, 0.0f);
                Gl.glVertex3f( size*0.5f, size, 0.0f);
            Gl.glEnd();
			
			Gl.glPopMatrix();
		}
	}
}
