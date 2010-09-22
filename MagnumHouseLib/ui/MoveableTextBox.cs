
using System;
using Tao.Sdl;

namespace MagnumHouseLib
{
	public class MoveableTextBox : TextBox
	{

		public MoveableTextBox (UserInput input, Vector2f size, Camera camera)
			: base (input, size, camera)
		{
		}
		
		public override void Update (float _delta)
		{
			if (Bounds.Contains(m_keyboard.MousePos) &&
			    m_keyboard.IsMouseButtonPressed(Sdl.SDL_BUTTON_LEFT))
			{
				CentreOn(m_keyboard.MousePos);
			}
			
			base.Update (_delta);
		}
	}
}
