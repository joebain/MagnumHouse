
using System;
using System.Drawing;
using System.Linq;
using Tao.Sdl;
namespace MagnumHouseLib
{
public class TitleScreen : Screen {
		
		public override void Setup (Game _game, UserInput _keyboard, ScreenMessage _message)
		{
			base.Setup(_game, _keyboard, _message);
			
			Sprite title = new Sprite(new Bitmap("pictures/title.png"));
			title.Position = new Vector2f();
			title.Size = new Vector2f(Game.Width, Game.Height);
			
			m_house.AddDrawable(title);
			
			Text text = new Text("[Click] or Press [Space] to Start");
			text.CentreOn(new Vector2f(Game.Width/2, Game.Height/5));
			m_house.AddDrawable(text);
		}
		
		public override void Update (float _delta)
		{
			if (m_keyboard.IsKeyPressed(Sdl.SDLK_SPACE) || m_keyboard.IsMouseButtonPressed(Sdl.SDL_BUTTON_LEFT)) {
				Exit(new ScreenMessage());
			}
		}
	}
}
