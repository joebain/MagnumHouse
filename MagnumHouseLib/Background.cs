using System;
using Tao.OpenGl;

namespace MagnumHouseLib
{
	public class Background : IDrawable
	{
		Vector3f[] points = new Vector3f[1000];
		Vector3f[] stars = new Vector3f[100];
		
		TileMap m_map;
		public float starSize = 2.0f;
		public Layer Layer { get { return Layer.Blurry; }}
		
		public Background(TileMap _map) {
			m_map = _map;
			
			Random rand = new Random(42);
			for (int i = 0 ; i < points.Length ; i++) {
				points[i] = new Vector3f((float)rand.NextDouble() * m_map.Width,
				                         (float)rand.NextDouble() * m_map.Height,
				                         ((float)rand.NextDouble() - 1f)*100000f - 1f);
			}
			for (int i = 0 ; i < stars.Length ; i++) {
				stars[i] = new Vector3f((float)rand.NextDouble() * m_map.Width,
				                         (float)rand.NextDouble() * m_map.Height,
				                         ((float)rand.NextDouble() - 1f)*100000f - 1f);
			}
		}
		
		public void Draw() {
			Gl.glPushMatrix();
			
			Gl.glEnable(Gl.GL_POINT_SMOOTH);
			Gl.glEnable(Gl.GL_LINE_SMOOTH);
			Gl.glEnable(Gl.GL_BLEND);
			Gl.glBlendFunc(Gl.GL_SRC_ALPHA,Gl.GL_ONE_MINUS_SRC_ALPHA);
			Gl.glHint(Gl.GL_POINT_SMOOTH_HINT, Gl.GL_DONT_CARE);
			Gl.glHint(Gl.GL_LINE_SMOOTH_HINT, Gl.GL_DONT_CARE);
			
			Gl.glPointSize(1f);
			Gl.glBegin(Gl.GL_POINTS);
			
			Random rand = new Random(17);
			foreach( var point in points) {
				float r = (float)rand.NextDouble()*0.039f;
				float g = (float)rand.NextDouble()*0.235f;
				Gl.glColor3f(0.961f+r,0.765f+g,0.537f);
				Gl.glVertex3f(point.X, point.Y, point.Z);
			}
			
			Gl.glEnd();
			
			
			
			
			Gl.glBegin(Gl.GL_LINES);
			foreach( var star in stars) {
				
				float r = (float)rand.NextDouble()*0.039f;
				float g = (float)rand.NextDouble()*0.235f;
				Gl.glColor3f(0.961f+r,0.765f+g,0.537f);
				Gl.glVertex3f(star.X, star.Y-starSize/4, star.Z); //top left
				Gl.glVertex3f(star.X+starSize, star.Y-starSize/4, star.Z); //top right
				Gl.glVertex3f(star.X+starSize, star.Y-starSize/4, star.Z); //top right
				Gl.glVertex3f(star.X+starSize/2, star.Y-starSize, star.Z); //bottom
				Gl.glVertex3f(star.X+starSize/2, star.Y-starSize, star.Z); //bottom
				Gl.glVertex3f(star.X, star.Y-starSize/4, star.Z); //top left
				
				Gl.glVertex3f(star.X+starSize/2, star.Y, star.Z); //top
				Gl.glVertex3f(star.X+starSize, star.Y-3*starSize/4, star.Z); //bottom right
				Gl.glVertex3f(star.X+starSize, star.Y-3*starSize/4, star.Z); //bottom right
				Gl.glVertex3f(star.X, star.Y-3*starSize/4, star.Z); //bottom left
				Gl.glVertex3f(star.X, star.Y-3*starSize/4, star.Z); //bottom left
				Gl.glVertex3f(star.X+starSize/2, star.Y, star.Z); //top
				
			}
			Gl.glEnd();
			
			Gl.glDisable(Gl.GL_BLEND);
			Gl.glDisable(Gl.GL_POINT_SMOOTH);
			Gl.glDisable(Gl.GL_LINE_SMOOTH);
			
			Gl.glPopMatrix();
		}
		
		public bool Dead { get { return false; }}
		
		public void Die() {}
	}
}
