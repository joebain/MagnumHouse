
using System;
using System.Drawing;
using Tao.OpenGl;

namespace MagnumHouseLib
{
	public class ScreenSprite : IUpdateable, IDrawable, IGrabing
	{
		public int Id { get; set; }
		
		Vector2i m_captureSize;
		Vector2i m_drawSize;
		Vector2f m_stepSize;
		Layer m_layerToCapture;
		float m_feedback = 0f;
		Sprite m_sprite;
		bool m_stickToScreen = false;
		Camera m_camera;
		
		bool spinning;
		bool zooming;
		float zoom_speed;
		float spin_speed;
		
		Vector2f cameraViewOffset = new Vector2f();
		
		
		public Action updateAction;
		
		public ScreenSprite(Vector2i captureSize, Vector2i screenSize, Vector2i drawSize) {
			m_captureSize = captureSize;
			m_drawSize = drawSize;
			
			m_stepSize = screenSize.ToF()/captureSize.ToF();
			
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
		
		public Layer CaptureLayer {
			set {
				m_layerToCapture = value;
			}
		}
		
		public void SetSpinning(float spin_speed) {
			spinning = true;
			this.spin_speed = spin_speed;
		}
		
		public void SetZooming(float zoom_speed) {
			zooming = true;
			this.zoom_speed = zoom_speed;
		}
		
		public Layer Layer { get { return Layer.Normal;	}}
		public Priority Priority {get ;set;}
		
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
			
			if (m_stickToScreen) {
				cameraViewOffset = m_camera.ViewOffset.Clone();
				m_camera.ViewOffset = m_camera.ViewOffset.Snap(m_stepSize);
				m_camera.SetPosition();
			}
			
			m_sprite.Draw();
			
			drawFunc(m_layerToCapture);
			
			if (m_stickToScreen) {
				m_camera.ViewOffset = cameraViewOffset;
				m_camera.SetPosition();
			}
			
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, m_sprite.TexNum);
            Gl.glCopyTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA, 0, 0, m_captureSize.X, m_captureSize.Y, 0);
            
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
			
			Gl.glViewport(initialViewportDimensions[0], 
			              initialViewportDimensions[1], 
			              initialViewportDimensions[2], 
			              initialViewportDimensions[3]);
			
			if (m_stickToScreen) {
				Position = Vector2f.Zero;
			}
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
		
		public void Die() {
		}
		public bool Dead {get { return false;}}
		
		public void Update(float _delta) {
			if (spinning) {
				m_sprite.Rotation += spin_speed * _delta;
			}
			if (zooming) {
				m_sprite.Zoom += zoom_speed * _delta;
			}
			if (updateAction != null) updateAction();
		}

	}
}
