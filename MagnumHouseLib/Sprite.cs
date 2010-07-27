using System;
using System.Drawing;
using Tao.OpenGl;

namespace MagnumHouseLib
{
	public class Sprite : Thing2D, IDrawable
	{		
		int m_texnum;
		public Bitmap Bitmap;
		
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
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, m_texnum);
			Gl.glTexParameteri(Gl.GL_TEXTURE_2D,Gl.GL_TEXTURE_MIN_FILTER,Gl.GL_LINEAR);
			Gl.glTexParameteri(Gl.GL_TEXTURE_2D,Gl.GL_TEXTURE_MAG_FILTER,Gl.GL_LINEAR);
		}
		
		public void ReloadBitmap() {
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, m_texnum);
			unsafe {
				var bits =  Bitmap.LockBits(
				                 new Rectangle(0,0,Bitmap.Width, Bitmap.Height),
				                 System.Drawing.Imaging.ImageLockMode.ReadOnly,
				                 System.Drawing.Imaging.PixelFormat.Format24bppRgb);
				Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB8, Bitmap.Width, Bitmap.Height, 0, Gl.GL_BGR, Gl.GL_UNSIGNED_BYTE , bits.Scan0);
				Bitmap.UnlockBits(bits);
			}
		}
		
		public void Draw() {
			Gl.glEnable(Gl.GL_TEXTURE_2D);
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, m_texnum);
			Gl.glColor3f(1,1,1);
			Gl.glPushMatrix();
			Gl.glTranslatef(Position.X, Position.Y, 0);
			Gl.glBegin(Gl.GL_TRIANGLE_STRIP);
			Gl.glVertex2f(0,0);
			Gl.glTexCoord2f(0,0);
			Gl.glVertex2f(0, Size.Y);
			Gl.glTexCoord2f(1, 1);
			Gl.glVertex2f(Size.X, 0);
			Gl.glTexCoord2f(1,0);
			Gl.glVertex2f(Size.X, Size.Y);
			Gl.glTexCoord2f(0,1);
			
			Gl.glEnd();
			Gl.glPopMatrix();
			
			Gl.glDisable(Gl.GL_TEXTURE_2D);
			//Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
		}
	}
}
