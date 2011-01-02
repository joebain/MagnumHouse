
using System;
using System.Drawing;
using Tao.OpenGl;

namespace MagnumHouseLib
{
	public class Effect : IUpdateable, IDrawable, IGrabing
	{
		public int Id { get; set; }
		
		Vector2i m_captureSize;
		public Vector2i CaptureSize { get { return m_captureSize; }}
		Vector2i m_drawSize;
		Vector2f m_stepSize;
		Layer m_layerToCapture;
		float m_feedback = 0f;
		Sprite s_sprite;
		bool m_stickToScreen = false;
		Camera m_camera;
		
		bool spinning;
		bool zooming;
		float zoom_speed;
		float spin_speed;
		bool fading;
		float fade_speed;
		Colour end_fade_colour;
		Colour start_fade_colour;
		float fade_level;
		Vector2i start_pixelling_dim;
		Vector2i end_pixelling_dim;
		IAnimator pixelling_speed;
		bool pixelling;
		bool draw_background;
		float[] bg_colour;
		
		Vector2f cameraViewOffset = new Vector2f();
		
		
		public Action updateAction;
		
		public Effect(Vector2i captureSize, Vector2i screenSize, Vector2i drawSize) {
			m_captureSize = captureSize;
			m_drawSize = drawSize;
			
			m_stepSize = captureSize.ToF()/screenSize.ToF();
			
			s_sprite = new Sprite(new Bitmap(m_captureSize.X, m_captureSize.Y));
			s_sprite.Size = new Vector2f(m_drawSize.X,m_drawSize.Y);
			s_sprite.YFlip = true;
		}
		
		public void SetHUD(Camera camera) {
			//m_sprite.SetHUD(camera);
			m_camera = camera;
			m_stickToScreen = true;	
		}
		
		public Sprite.ScaleType Scaling {
			set {
				s_sprite.Scaling = value;
				s_sprite.SetScalingParameters();
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
		
		public void SetFading(float fadeSpeed, Colour startFadeColour, Colour endFadeColour) {
			fade_level = 0;
			fading = true;
			fade_speed = fadeSpeed;
			end_fade_colour = endFadeColour;
			start_fade_colour = startFadeColour;
		}
		
		public void SetPixelling(IAnimator pixeling_speed, Vector2i start_dim, Vector2i end_dim) {
			pixelling = true;
			this.pixelling_speed = pixeling_speed;
			start_pixelling_dim = start_dim;
			end_pixelling_dim = end_dim;
		}
		
		public void SetBackground(float[] colour) {
			draw_background = true;
			bg_colour = colour;
		}
		
		private Layer m_layer = Layer.FX;
		public Layer Layer { get { return m_layer;} set {m_layer = value;}}
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
			
			if (pixelling) {
				float pixelling_level = pixelling_speed.Position;
				Vector2i viewport = new Vector2i(
				              (int)Math.Round(
				                              start_pixelling_dim.X * (1-pixelling_level) + 
				                              end_pixelling_dim.X * pixelling_level),
				              (int)Math.Round(
				                              start_pixelling_dim.Y * (1-pixelling_level) + 
				                              end_pixelling_dim.Y * pixelling_level));
				Gl.glViewport(0,0,viewport.X, viewport.Y);
				              
				s_sprite.CropFar =
					(start_pixelling_dim.ToF()/end_pixelling_dim.ToF()) * (1-pixelling_level) +
					Vector2f.Unit * pixelling_level;
			} else {
				Gl.glViewport(0,0, m_captureSize.X, m_captureSize.Y);
			}
			
			s_sprite.Transparency = m_feedback;
			
			if (m_stickToScreen) {
				cameraViewOffset = m_camera.ViewOffset.Clone();
				m_camera.ViewOffset = m_camera.ViewOffset.Snap(m_stepSize);
				m_camera.SetPosition();
			}
			
			s_sprite.Draw();
			
			drawFunc(m_layerToCapture);
			
			if (m_stickToScreen) {
				m_camera.ViewOffset = cameraViewOffset;
				m_camera.SetPosition();
			}
			
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, s_sprite.TexNum);
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
					s_sprite.Position = value - m_camera.ViewOffset;
					s_sprite.Position = s_sprite.Position.Snap(m_stepSize);
				} else {
					s_sprite.Position = value;
				}
			}
		}
		
		private void DrawColouredRectangle(float[] colour) {
			Gl.glBlendFunc(Gl.GL_SRC_ALPHA,Gl.GL_ONE_MINUS_SRC_ALPHA);
				Gl.glEnable(Gl.GL_BLEND);
				Gl.glPushMatrix();
				
				Gl.glColor4fv(colour);
			
				Gl.glTranslatef(s_sprite.Position.X + s_sprite.Size.X*0.5f, s_sprite.Position.Y + s_sprite.Size.Y*0.5f, 0);
				
				Gl.glScalef(s_sprite.Zoom, s_sprite.Zoom, 1);
				
				Gl.glRotatef(s_sprite.Rotation, 0, 0, 1);
				
				Gl.glBegin(Gl.GL_TRIANGLE_STRIP);
				Gl.glVertex3f(s_sprite.Size.X*-0.5f,s_sprite.Size.Y*-0.5f, s_sprite.Depth);
				Gl.glVertex3f(s_sprite.Size.X*-0.5f, s_sprite.Size.Y*0.5f, s_sprite.Depth);
				Gl.glVertex3f(s_sprite.Size.X*0.5f, s_sprite.Size.Y*-0.5f, s_sprite.Depth);
				Gl.glVertex3f(s_sprite.Size.X*0.5f, s_sprite.Size.Y*0.5f, s_sprite.Depth);
				
				Gl.glEnd();
				Gl.glPopMatrix();
				
				Gl.glDisable(Gl.GL_BLEND);
		}
		
		public void Draw ()
		{
			//draw bg
			if (draw_background) {
				DrawColouredRectangle(bg_colour);
			}
			
			s_sprite.Transparency = 1f;
			s_sprite.Draw();
			
			//draw fade
			if (fading) {
				DrawColouredRectangle(
				    Colour.Blend(start_fade_colour, end_fade_colour, fade_level).Array
				);
			}
		}
		
		private bool m_dead = false;
		
		public void Die() {
			m_dead = true;
		}
		public bool Dead {get { return m_dead;}}
		
		public virtual void Update(float _delta) {
			if (spinning) {
				s_sprite.Rotation += spin_speed * _delta;
			}
			if (zooming) {
				s_sprite.Zoom += zoom_speed * _delta;
			}
			if (fading) {
				fade_level += fade_speed * _delta;
			}
			if (pixelling) {
				pixelling_speed.Step(_delta);
			}
			if (updateAction != null) updateAction();
		}

	}
}
