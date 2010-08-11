using System;
using Tao.OpenGl;

namespace MagnumHouseLib
{
	public class Background : IDrawable
	{
		Vector3f[] points = new Vector3f[10000];
		
		TileMap m_map;
		
		public Layer Layer { get { return Layer.Blurry; }}
		
		public Background(TileMap _map) {
			m_map = _map;
			
			Random rand = new Random(42);
			for (int i = 0 ; i < points.Length ; i++) {
				points[i] = new Vector3f((float)rand.NextDouble() * m_map.Width, (float)rand.NextDouble() * m_map.Height, ((float)rand.NextDouble() - 1f)*1000f);
			}
		}
		
		public void Draw() {
			Gl.glPushMatrix();
			
			Gl.glBegin(Gl.GL_POINTS);
			Random rand = new Random(17);
			foreach( var point in points) {
				float r = (float)rand.NextDouble()*0.039f;
				float g = (float)rand.NextDouble()*0.235f;
				Gl.glColor3f(0.961f+r,0.765f+g,0.537f);
				Gl.glVertex3f(point.X, point.Y, point.Z);
			}
			
			Gl.glEnd();
			
			Gl.glPopMatrix();
		}
		
		public bool Dead { get { return false; }}
		
		public void Die() {}
	}
}
