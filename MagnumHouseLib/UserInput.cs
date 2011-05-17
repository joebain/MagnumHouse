using System;
using System.Collections.Generic;

using Tao.Sdl;
using System.Text;

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
		
		public Boolean WindowHasFocus {
			get {
				byte state = Sdl.SDL_GetAppState ();
				return (state & Sdl.SDL_APPMOUSEFOCUS) != 0 && (state & Sdl.SDL_APPINPUTFOCUS) != 0 && (state & Sdl.SDL_APPACTIVE) != 0;
			}
		}
		
		public Action<Sdl.SDL_keysym> KeyDown;
		public Action<Sdl.SDL_keysym> KeyUp;
		public Action<Sdl.SDL_MouseButtonEvent> MouseDown;
		public Action<Sdl.SDL_MouseButtonEvent> MouseUp;
		
		public bool KeyIsChar(Sdl.SDL_keysym key) {
			char letter = KeyToChar(key);
			if (Text.characters.Contains(letter.ToString()))
				return true;
			return false;
		}
		
		public bool KeyIsNumber(Sdl.SDL_keysym key) {
			char letter = KeyToChar(key);
			int i;
			if (int.TryParse(letter.ToString(), out i))
				return true;
			return false;
		}
		
		public char KeyToChar(Sdl.SDL_keysym key) {
			if( key.unicode < 0x80 && key.unicode > 0 ){
				return (char) key.unicode;
			}
			return '\0';
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
					if (KeyDown != null) KeyDown(e.key.keysym);
					if (e.key.keysym.sym == Sdl.SDLK_ESCAPE) m_quitFlag = true;
				}
				else if (e.type == Sdl.SDL_KEYUP)
				{
					m_keys[e.key.keysym.sym] = false;
					if (KeyUp != null) KeyUp(e.key.keysym);
				}
				else if (e.type == Sdl.SDL_MOUSEBUTTONDOWN) {
					m_mouseButtons[e.button.button] = true;
					if (MouseDown != null) MouseDown(e.button);
				}
				else if (e.type == Sdl.SDL_MOUSEBUTTONUP) {
					m_mouseButtons[e.button.button] = false;
					if (MouseUp != null) MouseUp(e.button);
				}
				else if (e.type == Sdl.SDL_MOUSEMOTION) {
					screenMousePos.X = e.motion.x;
					screenMousePos.Y = e.motion.y;
				}
			}
		}
	}
}
