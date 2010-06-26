using System;
using System.Collections.Generic;

using Tao.Sdl;

namespace MagnumHouse
{
	public class UserInput
	{
		bool m_quitFlag = false;
		
		Dictionary<int, bool> m_keys = new Dictionary<int, bool>();
		Dictionary<byte, bool> m_mouseButtons = new Dictionary<byte, bool>();
		
		Vector2i m_mousePos = new Vector2i();
		
		public Vector2i MousePos { get { return m_mousePos.Clone(); }}
		
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
		
		public UserInput()
		{
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
					m_mousePos.X = e.motion.x;
					m_mousePos.Y = e.motion.y;
				}
			}
		}
	}
}
