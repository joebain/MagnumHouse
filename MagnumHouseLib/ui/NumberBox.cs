
using System;
using Tao.Sdl;

namespace MagnumHouseLib
{
	public class NumberBox : TextBox
	{
		public NumberBox (UserInput input, Vector2f size, Camera camera)
			: base (input, size, camera)
		{
		}
		
		public double Number {
			get {
				return double.Parse(Text);
			}
			set {
				Text = value.ToString();
			}
		}
		
		public override void AddLetter (Tao.Sdl.Sdl.SDL_keysym key)
		{
			if (m_keyboard.KeyIsNumber(key) || m_keyboard.KeyToChar(key) == '.' || key.sym == Sdl.SDLK_BACKSPACE) {
				base.AddLetter (key);
			}
		}
	}
}
