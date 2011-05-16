
using System;
using Tao.Sdl;

namespace MagnumHouseLib
{
	public class ColourTextBox : TextBox
	{
		public ColourTextBox (UserInput input, Vector2f size, Camera camera)
			: base (input, size, camera)
		{
		}
		
		public Colour Colour {
			get {
				return Colour.Parse(Text);
			}
			set {
				Text = value.ToString();
			}
		}
		
		public override void AddLetter (Tao.Sdl.Sdl.SDL_keysym key)
		{
			char letter = m_keyboard.KeyToChar(key);
			if (m_keyboard.KeyIsNumber(key) ||
			    letter == ','|| key.sym == Sdl.SDLK_BACKSPACE) {
				base.AddLetter (key);
			}
		}
	}
}
