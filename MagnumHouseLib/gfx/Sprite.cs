using System;
using System.Drawing;
using Tao.OpenGl;

namespace MagnumHouseLib
{
	public class Sprite : Thing2D, IDrawable
	{
		public Layer Layer { get { return Layer.Normal; }}
		public Priority Priority {get { return Priority.Middle; } }
		
		int m_texnum;
		public int TexNum { get { return m_texnum; }}
		public Bitmap Bitmap;
		
		public float Transparency = 1.0f;
		public bool XFlip = false;
		public bool YFlip = false;
		
		public float Rotation = 0f;
		public float Zoom = 1f;
		
		public enum ScaleType {
			Pixelly, Blurry
		}
		
		public ScaleType Scaling = ScaleType.Blurry;
		
		public Sprite() : this(new Bitmap(1,1))
		{
		}
		
		public Sprite (Bitmap _bitmap)
		{
			Bitmap = _bitmap;
			InitTex();
			ReloadBitmap();
		}
		
		private void InitTex() {
			Gl.glGenTextures(1, out m_texnum);
			SetScalingParameters();
		}
		
		public void SetScalingParameters() {
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, m_texnum);
			if (Scaling == ScaleType.Blurry) {
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D,Gl.GL_TEXTURE_MIN_FILTER,Gl.GL_LINEAR);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D,Gl.GL_TEXTURE_MAG_FILTER,Gl.GL_LINEAR);
			} else {
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D,Gl.GL_TEXTURE_MIN_FILTER,Gl.GL_NEAREST);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D,Gl.GL_TEXTURE_MAG_FILTER,Gl.GL_NEAREST);
			}
		}
		
		public void ReloadBitmap() {
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, m_texnum);
			unsafe {
				var bits =  Bitmap.LockBits(
				                 new Rectangle(0,0,Bitmap.Width, Bitmap.Height),
				                 System.Drawing.Imaging.ImageLockMode.ReadOnly,
				                 System.Drawing.Imaging.PixelFormat.Format32bppArgb);
				Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA, Bitmap.Width, Bitmap.Height, 0, Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE , bits.Scan0);
				Bitmap.UnlockBits(bits);
			}
		}
		
		public void Draw() {
			Gl.glEnable(Gl.GL_TEXTURE_2D);
			
			Gl.glBlendFunc(Gl.GL_SRC_ALPHA,Gl.GL_ONE_MINUS_SRC_ALPHA);
			Gl.glEnable(Gl.GL_BLEND);
			
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, m_texnum);
			Gl.glColor4f(1,1,1, Transparency);
			Gl.glPushMatrix();
			
			Gl.glTranslatef(Size.X/2, Size.Y/2, 0);
			Gl.glRotatef(Rotation, 0, 0, 1);
			Gl.glScalef(Zoom, Zoom, 1);
			Gl.glTranslatef(-Size.X/2, -Size.Y/2, 0);
			
			Gl.glTranslatef(Position.X, Position.Y, 0);

			Gl.glMatrixMode(Gl.GL_TEXTURE);
			Gl.glPushMatrix();
			Gl.glLoadIdentity();
			if (YFlip) {
				Gl.glScalef(1f, -1f, 1f);
			}
			if (XFlip) {
				Gl.glScalef(-1f, 1f, 1f);
			}
			Gl.glMatrixMode(Gl.GL_MODELVIEW);
			
			Gl.glBegin(Gl.GL_TRIANGLE_STRIP);
			Gl.glVertex3f(0,0, Depth);
			Gl.glTexCoord2f(0,0);
			Gl.glVertex3f(0, Size.Y, Depth);
			Gl.glTexCoord2f(1, 1);
			Gl.glVertex3f(Size.X, 0, Depth);
			Gl.glTexCoord2f(1,0);
			Gl.glVertex3f(Size.X, Size.Y, Depth);
			Gl.glTexCoord2f(0,1);
			
			Gl.glEnd();
			Gl.glPopMatrix();
			
			Gl.glDisable(Gl.GL_BLEND);
			Gl.glDisable(Gl.GL_TEXTURE_2D);
			//Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
		}
	}
}
