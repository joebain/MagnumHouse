
using System;
using System.Drawing;
using Tao.OpenGl;

namespace MagnumHouseLib
{
	public class ScreenSprite
	{
		Vector2i m_captureSize;
		Vector2i m_drawSize;
		Vector2f m_stepSize;
		Layer m_layer;
		float m_feedback = 0f;
		Sprite m_sprite;
		bool m_stickToScreen = false;
		Camera m_camera;
		
		public ScreenSprite(Vector2i captureSize, Vector2i screenSize, Vector2i drawSize) {
			m_captureSize = captureSize;
			m_drawSize = drawSize;
			
			m_stepSize = captureSize.ToF()/drawSize.ToF();
			Console.WriteLine("draw size : " + drawSize);
			Console.WriteLine("capture size : " + captureSize);
			Console.WriteLine("step is : " + m_stepSize);
			
			m_sprite = new Sprite(new Bitmap(m_captureSize.X, m_captureSize.Y));
			m_sprite.Size = new Vector2f(m_drawSize.X,m_drawSize.Y);
			m_sprite.YFlip = true;
		}
		
		public void SetHUD(Camera camera) {
			//m_sprite.SetHUD(camera);
			m_camera = camera;
			m_stickToScreen = true;	
		}
		
		public Sprite.ScaleType Scaling {
			set {
				m_sprite.Scaling = value;
				m_sprite.SetScalingParameters();
			}
		}
		
		public Layer Layer {
			set {
				m_layer = value;
			}
		}
		
		public float Feedback {
			set {
				m_feedback = value;
			}
		}
		
		public void Grab(Action<Layer> drawFunc) {
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
			
			//get current viewport size
			int[] initialViewportDimensions = new int[4];
			Gl.glGetIntegerv(Gl.GL_VIEWPORT, initialViewportDimensions);
			
			Gl.glViewport(0,0,m_captureSize.X, m_captureSize.Y);
			
			m_sprite.Transparency = m_feedback;
			m_sprite.Draw();
			Vector2f viewOffset = Vector2f.Zero;
			if (m_stickToScreen) {
				viewOffset = m_camera.ViewOffset.Clone();
				m_camera.ViewOffset = m_camera.ViewOffset.Snap(m_stepSize);
				m_camera.SetPosition();
			}
			
			drawFunc(m_layer);
			
			if (m_stickToScreen) {
				m_camera.ViewOffset = viewOffset;
				m_camera.SetPosition();
			}
			
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, m_sprite.TexNum);
            Gl.glCopyTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA, 0, 0, m_captureSize.X, m_captureSize.Y, 0);
            
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
			
			Gl.glViewport(initialViewportDimensions[0], 
			              initialViewportDimensions[1], 
			              initialViewportDimensions[2], 
			              initialViewportDimensions[3]);
		}
		
		public Vector2f Position {
			set {
				
				
				if (m_stickToScreen) {
					m_sprite.Position = value - m_camera.ViewOffset;
					m_sprite.Position = m_sprite.Position.Snap(m_stepSize);
				} else {
					m_sprite.Position = value;
				}
			}
		}
		
		public void Draw ()
		{
			m_sprite.Transparency = 1f;
			m_sprite.Draw();
		}

	}
}
