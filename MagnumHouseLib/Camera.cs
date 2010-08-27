
using System;
using Tao.OpenGl;

namespace MagnumHouseLib
{
	public class Camera
	{

		public Thing2D CameraSubject;
		public Vector2i ScreenSize;
		public Vector2f ViewOffset = Game.ScreenSize*0.5f;
		public Vector2f LastOffset = Game.ScreenSize*0.5f;
		
		public void SetPosition() {
			Gl.glMatrixMode(Gl.GL_PROJECTION);
			Gl.glLoadIdentity();
			
			Gl.glFrustum(10,20, 10, 20, 0, 1000);
			//Glu.gluPerspective(45, (float)Width/Height, 0.1, 10);
			
			
			Gl.glMatrixMode(Gl.GL_MODELVIEW);
			Gl.glLoadIdentity();
			
			Gl.glTranslatef(-1.0f,-1.0f,0f);
			Gl.glScalef(2.0f/Game.Width,2.0f/Game.Height,0f);
			Gl.glScalef(Game.Zoom, Game.Zoom, 0f);
			
			Gl.glTranslatef(ViewOffset.X, ViewOffset.Y, 0);	
		}
		
		public Vector2f FindOffset() {
			LastOffset = ViewOffset;
			
			Vector2f centre;
			
			if (CameraSubject != null)
				centre = CameraSubject.Position;
			else
				centre = Game.ScreenCentre;
			
			ViewOffset = Game.ScreenSize*0.5f - centre;
			
			Vector2i size;
			if (ScreenSize == null) {
				size = new Vector2i(Game.ScreenWidth, Game.ScreenHeight);
			} else {
				size = ScreenSize;
			}
			
			if (ViewOffset.X < -size.X+Game.Width) {
				ViewOffset.X = -size.X+Game.Width;
			}
			else if (ViewOffset.X > 0) {
				ViewOffset.X = 0;
			}
			if (ViewOffset.Y < -size.Y+Game.Height) {
				ViewOffset.Y = -size.Y+Game.Height;
			}
			else if (ViewOffset.Y > 0) {
				ViewOffset.Y = 0;
			}
			
			return ViewOffset;
		}
		
	}
}
