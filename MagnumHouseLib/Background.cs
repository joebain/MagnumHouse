using System;
using Tao.OpenGl;

namespace MagnumHouse
{
	public class Background : IDrawable
	{
		Vector2f[] points = new Vector2f[10000];
		
		TileMap m_map;
		
		public Background(TileMap _map) {
			m_map = _map;
			
			Random rand = new Random(42);
			for (int i = 0 ; i < points.Length ; i++) {
				points[i] = new Vector2f((float)rand.NextDouble() * m_map.Width, (float)rand.NextDouble() * m_map.Height);
			}
		}
		
		public void Draw() {
			Gl.glPushMatrix();
			
			Gl.glBegin(Gl.GL_POINTS);
			Gl.glColor3f(1,1,1);
			foreach( var point in points) {
				Gl.glVertex3f(point.X, point.Y, -0.1f);
			}
			
			Gl.glEnd();
			
			Gl.glPopMatrix();
		}
		
		public bool Dead { get { return false; }}
	}
}
