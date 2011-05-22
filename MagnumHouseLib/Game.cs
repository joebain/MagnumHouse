
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
		public const int ScreenWidth = 1024;
		public const int ScreenHeight = 800;
		
		
		public const int bg_pixel_size = 2;
		public static int SmallScreenWidth {get {return ScreenWidth / bg_pixel_size;}}
		public static int SmallScreenHeight {get {return ScreenHeight / bg_pixel_size;}}
		public static int Width {get { return ScreenWidth / Tile.Size; }}
		public static int Height {get { return  ScreenHeight / Tile.Size; }}
		public static Vector2i Size {get { return new Vector2i(Width, Height);}}
		public static float Zoom = 1.0f;
		public static readonly Vector2f ScreenCentre = new Vector2f(Width/2f, Height/2f);
		public static readonly Vector2f ScreenSize = new Vector2f(Width, Height);
		
		private Camera m_camera = new Camera();
		public Camera Camera { get { return m_camera; } }
		
		UserInput m_keyboard;
			
		const long targetMsPerFrame = 16;
		const long maxMsPerFrame = 33;
		int effect_counter = 0;
		
		private bool m_stepping = false;
		bool quitFlag = false;
		bool restartFlag = false;
		
		private IEnumerable<Screen> m_screens;
		private ScreenMessage m_lastMessage;
		
		public Game() {
			m_keyboard = new UserInput(this);
		}
		
		public void SetCameraSubject(Thing2D _subject) {
			m_camera.CameraSubject = _subject;
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
		
		public void SetLevels(IEnumerable<Screen> _levels) {
			m_screens = _levels;
		}
		
		public void AddLevel(Screen _level) {
			List<Screen> levels = m_screens.ToList();
			levels.Add(_level);
			m_screens = levels;
		}
		
		public void Setup()
		{	
			if (m_screens == null) {
				List<Screen> screens = new List<Screen>();
				screens.Add(new TitleScreen());
				//screens.Add(new TrailLevel());
				//screens.Add(new PlatformLevel("pictures/platformlevel.png"));
				screens.Add(new LevelOne());
				//screens.Add(new NetworkLevel());
				screens.Add(new EndScreen());
				m_screens = screens;
			}
			
			InitGfx();
			Sdl.SDL_EnableUNICODE(1);
			Text.Init();
			
			LoadScreen(new ScreenMessage());
		}
		
		private void LoadScreen(ScreenMessage _message) {
			m_lastMessage = _message;
			m_screens.First().Setup(this, m_keyboard, _message);
			m_screens.First().ExitRequest += NextScreen;
			m_screens.First().ReloadRequest += ReloadScreen;
			m_camera.CurrentScreen = m_screens.First();
		}
		
		private void ReloadScreen(ScreenMessage _message) {
			m_screens.First().Setup(this, m_keyboard, _message);
		}
		
		private void NextScreen(ScreenMessage _message) {
			m_screens.First().ExitRequest -= NextScreen;
			m_screens.First().ReloadRequest -= ReloadScreen;
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
                
                Gl.glClearColor(0f, 0f, 0f, 0f);
				
				if (restartFlag) {
					restartFlag = false;
					ReloadScreen(new ScreenMessage());
				}
				
				long deltaMillisecs
					= time.ElapsedMilliseconds > maxMsPerFrame
						? maxMsPerFrame
						: time.ElapsedMilliseconds;
                float delta = deltaMillisecs/1000.0f;
				
				time.Reset();
				time.Start();
				m_screens.First().House.Update(delta);
				m_screens.First().Update(delta);
				
				m_camera.FindOffset();
				m_camera.SetPosition();
				
				m_screens.First().House.Grab();
				
				Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
				
				m_screens.First().House.Draw(Layer.FX, Priority.Back);
				m_screens.First().House.Draw(Layer.Normal);
				m_screens.First().House.Draw(Layer.FX, Priority.Middle);
				m_screens.First().House.Draw(Layer.FX, Priority.Front);
				
				Sdl.SDL_GL_SwapBuffers();
				
				long sleep = targetMsPerFrame-time.ElapsedMilliseconds;
				if (sleep > 0) Thread.Sleep(TimeSpan.FromMilliseconds(sleep));
            }
        }
		
		public Vector2f ScreenPxToGameCoords(Vector2i _px) {
			return new Vector2f(
				(float)_px.X / (Tile.Size*Zoom) - m_camera.ViewOffset.X,
			    (float)(ScreenHeight-_px.Y) / (Tile.Size*Zoom) - m_camera.ViewOffset.Y);
		}
	}
}
