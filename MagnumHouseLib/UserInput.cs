using System;
using System.Collections.Generic;

using Tao.Sdl;

namespace MagnumHouseLib
{
	public class UserInput
	{
		public int Id { get; set;}
		
		bool m_quitFlag = false;
		
		Dictionary<int, bool> m_keys = new Dictionary<int, bool>();
		Dictionary<byte, bool> m_mouseButtons = new Dictionary<byte, bool>();
		
		Vector2f m_mousePos = new Vector2f();
		Vector2i screenMousePos = new Vector2i();
		
		public Vector2f MousePos { get { return m_mousePos.Clone(); }}
		
		public bool QuitRequested { get { return m_quitFlag; }}
		
		public bool IsKeyPressed(int _key) {
			bool isPressed;
			if (m_keys.TryGetValue(_key, out isPressed))
			{
				return isPressed;
			}
			else
			{
				return false;
			}
		}
		
		public bool IsMouseButtonPressed(byte _button) {
			bool isPressed;
			if (m_mouseButtons.TryGetValue(_button, out isPressed)) {
				return isPressed;
			} else {
				return false;
			}
		}
		
		Game m_game;
		
		public UserInput(Game _game)
		{
			m_game = _game;
		}
		
		public void Update(float _delta) {
			m_mousePos = m_game.ScreenPxToGameCoords(screenMousePos);
		}
		
		public void HandleSDLInput()
		{
			Sdl.SDL_Event e;
			
			while (Sdl.SDL_PollEvent(out e) != 0) {
	        
				if (e.type == Sdl.SDL_QUIT)
				{
	                m_quitFlag = true;
				}
				else if (e.type == Sdl.SDL_KEYDOWN)
				{
					m_keys[e.key.keysym.sym] = true;
					if (e.key.keysym.sym == Sdl.SDLK_ESCAPE) m_quitFlag = true;
				}
				else if (e.type == Sdl.SDL_KEYUP)
				{
					m_keys[e.key.keysym.sym] = false;
				}
				else if (e.type == Sdl.SDL_MOUSEBUTTONDOWN) {
					m_mouseButtons[e.button.button] = true;
				}
				else if (e.type == Sdl.SDL_MOUSEBUTTONUP) {
					m_mouseButtons[e.button.button] = false;
				}
				else if (e.type == Sdl.SDL_MOUSEMOTION) {
					screenMousePos.X = e.motion.x;
					screenMousePos.Y = e.motion.y;
				}
			}
		}
	}
}
