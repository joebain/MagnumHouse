
using System;
using Tao.OpenGl;

namespace MagnumHouseLib
{
	public class Camera
	{

		public Thing2D CameraSubject;
		public Vector2f ViewOffset = Game.ScreenSize*0.5f;
		public Vector2f LastOffset = Game.ScreenSize*0.5f;
		
		public BoundingBox Bounds = new BoundingBox(0,0,Game.Width, Game.Height);
		
		public Screen CurrentScreen;
		
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
			
			if (CameraSubject != null) {
				centre = CameraSubject.Position;
			} else if (CurrentScreen != null) {
				centre = CurrentScreen.Bounds.Centre.Clone();
			} else {
				centre = Game.ScreenCentre;
			}
			
			ViewOffset = Game.ScreenSize*0.5f - centre;
			
			if (CurrentScreen == null) return ViewOffset;
			
			Bounds = CurrentScreen.Bounds.Clone;
			Bounds.Right -= Game.Width;
			//Bounds.Left += Game.Width/2;
			Bounds.Top -= Game.Height;
			//Bounds.Bottom += Game.Height/2;
			
			ViewOffset *= -1;
			ViewOffset.Clamp(Bounds);
			ViewOffset *= -1;
			
			return ViewOffset;
		}
		
	}
}
