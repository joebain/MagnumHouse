
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Drawing;
using Tao.OpenGl;
using Tao.Sdl;
using Tao.FreeGlut;

using MagnumHouseLib;

namespace MagnumHouseLib
{
	public class Game
	{
		public const int ScreenWidth = 800;
		public const int ScreenHeight = 600;
		
		public const int bg_pixel_size = 4;
		public static int SmallScreenWidth {get {return ScreenWidth / bg_pixel_size;}}
		public static int SmallScreenHeight {get {return ScreenHeight / bg_pixel_size;}}
		public static int Width {get { return ScreenWidth / Tile.Size; }}
		public static int Height {get { return  ScreenHeight / Tile.Size; }}
		public float Zoom = 1.0f;
		public static readonly Vector2f ScreenCentre = new Vector2f(Width/2f, Height/2f);
		public static readonly Vector2f ScreenSize = new Vector2f(Width, Height);
		
		public Vector2f viewOffset = new Vector2f();
		
		UserInput m_keyboard;
			
		const long targetMsPerFrame = 16;
		int effect_counter = 0;
		
		private bool m_stepping = false;
		bool quitFlag = false;
		bool restartFlag = false;
		
		private IEnumerable<Screen> m_screens;
		private ScreenMessage m_lastMessage;
		
		private Thing2D m_cameraSubject;
		private Vector2f m_prevSubjectLoc = new Vector2f();
		
		private Sprite pixelly_fx_buffer;
		private Sprite blurry_fx_buffer;
		
		
		public Game() {
			m_keyboard = new UserInput(this);
		}
		
		public void SetCameraSubject(Thing2D _subject) {
			m_cameraSubject = _subject;
		}
		
		public void Restart() {
			restartFlag = true;	
		}
		
		
		public event Action Quitting;
		
		public void Quit() {
			quitFlag = true;
			m_screens.First().House.Die();
			if (Quitting != null) Quitting();
		}
		
		private void InitGfx() {
			Sdl.SDL_Init(Sdl.SDL_INIT_EVERYTHING);
            Sdl.SDL_GL_SetAttribute(Sdl.SDL_GL_DOUBLEBUFFER, 1);
            Sdl.SDL_GL_SetAttribute(Sdl.SDL_GL_DEPTH_SIZE, 16);
            Sdl.SDL_GL_SetAttribute(Sdl.SDL_GL_RED_SIZE, 8);
            Sdl.SDL_GL_SetAttribute(Sdl.SDL_GL_GREEN_SIZE, 8);
            Sdl.SDL_GL_SetAttribute(Sdl.SDL_GL_BLUE_SIZE, 8);
            Sdl.SDL_GL_SetAttribute(Sdl.SDL_GL_ALPHA_SIZE, 8);
            int flags = (Sdl.SDL_HWSURFACE | Sdl.SDL_OPENGL);
            IntPtr contextPointer = Sdl.SDL_SetVideoMode(ScreenWidth, ScreenHeight, 0, flags);
            //Sdl.SDL_Surface context = (Sdl.SDL_Surface)
                    Marshal.PtrToStructure(contextPointer,
                    typeof(Sdl.SDL_Surface));
			
			Gl.glEnable(Gl.GL_TEXTURE_2D);
		}
		
		public void Setup(IEnumerable<Screen> screens) {
			InitGfx();
			
			pixelly_fx_buffer = new Sprite(new Bitmap(SmallScreenWidth, SmallScreenHeight));
			pixelly_fx_buffer.Size = new Vector2f(Width,Height);
			pixelly_fx_buffer.YFlip = true;
			pixelly_fx_buffer.Transparency = 0.7f;
			pixelly_fx_buffer.Scaling = Sprite.ScaleType.Pixelly;
			pixelly_fx_buffer.SetParameters();
			
			blurry_fx_buffer = new Sprite(new Bitmap(SmallScreenWidth, SmallScreenHeight));
			blurry_fx_buffer.SetHUD(this);
			blurry_fx_buffer.Size = new Vector2f(Width,Height);
			blurry_fx_buffer.YFlip = true;
			blurry_fx_buffer.Transparency = 0.7f;
			blurry_fx_buffer.Scaling = Sprite.ScaleType.Blurry;
			blurry_fx_buffer.SetParameters();

			m_screens = screens;
			LoadScreen(new ScreenMessage());
		}
		
		public void Setup()
		{			
			var screens = new List<Screen>();
			screens.Add(new TitleScreen());
			screens.Add(new PlatformLevel("pictures/platformlevel.png"));
			screens.Add(new EndScreen());
			//screens.Add(new NetworkLevel());
			
			Setup(screens);
		}
		
		private void LoadScreen(ScreenMessage _message) {
			m_lastMessage = _message;
			m_screens.First().Setup(this, m_keyboard, _message);
			m_screens.First().Exiting += NextScreen;
		}
		
		private void ReLoadScreen() {
			m_screens.First().Setup(this, m_keyboard, m_lastMessage);
		}
		
		private void NextScreen(ScreenMessage _message) {
			m_screens.First().Exiting -= NextScreen;
			m_screens = m_screens.Skip(1);
			if (!m_screens.Any()) {
				quitFlag = true;
				return;
			}
			LoadScreen(_message);
		}
		
		public void Run()
		{
			
            Stopwatch time = new Stopwatch();
			time.Start();
            while (quitFlag == false)
            {
                m_keyboard.HandleSDLInput();
				m_keyboard.Update(0);
				
				if (m_keyboard.QuitRequested) {
					Quit();
				}
                
                Gl.glClearColor(0f, 0f, 0f, 1f);
                //Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
				
				if (restartFlag || m_keyboard.IsKeyPressed(Sdl.SDLK_r)) {
					restartFlag = false;
					ReLoadScreen();
				}
				
				long deltaMillisecs = time.ElapsedMilliseconds;
                float delta = deltaMillisecs/1000.0f;
				
				if (m_keyboard.IsKeyPressed(Sdl.SDLK_o)) {
					m_stepping = !m_stepping;
				}
				if (m_stepping) Thread.Sleep(500);
				
				if (m_cameraSubject != null)
					m_prevSubjectLoc = m_cameraSubject.Position;
				
				time.Reset();
				time.Start();
				m_screens.First().House.Update(delta);
				m_screens.First().Update(delta);

				
				
				GrabEffectsBuffer();
				
				if (m_cameraSubject != null)
					Camera(m_cameraSubject.Position);
				else
					Camera(ScreenCentre);
				
				DrawEffectsBuffer();
				
				m_screens.First().House.Draw(Layer.Normal);
                
				
				Sdl.SDL_GL_SwapBuffers();
				
				long sleep = targetMsPerFrame-time.ElapsedMilliseconds;
				if (sleep > 0) Thread.Sleep(TimeSpan.FromMilliseconds(sleep));
            }
        }
		
		private void DrawEffectsBuffer() {
			if (m_cameraSubject != null) {
				pixelly_fx_buffer.Position = m_prevSubjectLoc - ScreenSize*0.5f;
			}
			float tmpPixellyTransparency = pixelly_fx_buffer.Transparency;
			pixelly_fx_buffer.Transparency = 1.0f;
			pixelly_fx_buffer.Draw();
			pixelly_fx_buffer.Transparency = tmpPixellyTransparency;
			
			
			float tmpBlurryTransparency = blurry_fx_buffer.Transparency;
			blurry_fx_buffer.Transparency = 0.7f;
			blurry_fx_buffer.Draw();
			blurry_fx_buffer.Transparency = tmpBlurryTransparency;
			
			
		}
		
		private void GrabEffectsBuffer() {
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
			Gl.glViewport(0,0,SmallScreenWidth, SmallScreenHeight);
			
			if (m_cameraSubject != null) {
				Camera(m_cameraSubject.Position);
			}
			pixelly_fx_buffer.Draw();
			m_screens.First().House.Draw(Layer.Pixelly);
			
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, pixelly_fx_buffer.TexNum);
            Gl.glCopyTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA, 0, 0, SmallScreenWidth, SmallScreenHeight, 0);
            
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
			
			if (m_cameraSubject != null) {
				Camera(m_cameraSubject.Position);
			}
			blurry_fx_buffer.Draw();
			m_screens.First().House.Draw(Layer.Blurry);
			
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, blurry_fx_buffer.TexNum);
            Gl.glCopyTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA, 0, 0, SmallScreenWidth, SmallScreenHeight, 0);
			
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
			
			Gl.glViewport(0,0,ScreenWidth, ScreenHeight);
		}
		
		public void Camera(Vector2f centre) {
			
			Gl.glMatrixMode(Gl.GL_PROJECTION);
			Gl.glLoadIdentity();
			
			
			viewOffset = ScreenSize*0.5f - centre;
			
			Gl.glFrustum(10,20, 10, 20, 0, 1000);
			//Glu.gluPerspective(45, (float)Width/Height, 0.1, 10);
			
			
			
			Gl.glMatrixMode(Gl.GL_MODELVIEW);
			Gl.glLoadIdentity();
			
			//Gl.glRotatef((float)Math.PI, 0, 0, 0);
			
			Gl.glTranslatef(-1.0f,-1.0f,0f);
			Gl.glScalef(2.0f/Width,2.0f/Height,0f);
			Gl.glScalef(Zoom, Zoom, 0f);
			
			Gl.glTranslatef(viewOffset.X, viewOffset.Y, 0);
			
			
		}
		
		public Vector2f ScreenPxToGameCoords(Vector2i _px) {
			return new Vector2f(
				(float)_px.X / (Tile.Size*Zoom) - viewOffset.X,
			    (float)(ScreenHeight-_px.Y) / (Tile.Size*Zoom) - viewOffset.Y);
		}
	}
}
