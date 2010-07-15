
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Drawing;
using Tao.OpenGl;
using Tao.Sdl;

using MagnumHouseLib;

namespace MagnumHouse
{
	public class Game
	{
		public const int ScreenWidth = 800;
		public const int ScreenHeight = 600;
		public static int Width {get { return ScreenWidth / Tile.Size; }}
		public static int Height {get { return  ScreenHeight / Tile.Size; }}
		public float Zoom = 1.0f;
		
		public Vector2f viewOffset = new Vector2f();
		
		UserInput m_keyboard = new UserInput();
			
		const long targetMsPerFrame = 16;
		
		private bool m_stepping = false;
		bool quitFlag = false;
		bool restartFlag = false;
		
		private IEnumerable<Screen> m_screens;
		private ScreenMessage m_lastMessage;
		
		
		public void Restart() {
			restartFlag = true;	
		}
		
		
		public event Action Quitting;
		
		public void Quit() {
			quitFlag = true;
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
			
			m_screens = screens;
			LoadScreen(new ScreenMessage());
		}
		
		public void Setup()
		{			
			var screens = new List<Screen>();
			screens.Add(new TitleScreen());
			//m_screens.Add(new TargetLevel());
			screens.Add(new NetworkLevel());
			//m_screens.Add(new EndScreen());
			
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
				if (m_keyboard.QuitRequested) {
					Quit();
				}
                
                Gl.glClearColor(0f, 0f, 0f, 1f);
                Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
				
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
					
				time.Reset();
				time.Start();
				m_screens.First().House.Update(delta);
				m_screens.First().Update(delta);
				Camera();
				m_screens.First().House.Draw();
                
                Sdl.SDL_GL_SwapBuffers();
				
				long sleep = targetMsPerFrame-time.ElapsedMilliseconds;
				if (sleep > 0) Thread.Sleep(TimeSpan.FromMilliseconds(sleep));
            }
        }
		
		public void Camera() {
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
