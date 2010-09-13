
using System;
using System.Drawing;
using System.Linq;
using Tao.Sdl;
namespace MagnumHouseLib
{
public class TitleScreen : Screen {
		
		float endTimer = -1;
		Effect fx_buffer;
		
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
			
			fx_buffer
				= new Effect(
				    new Vector2i(Game.ScreenWidth, Game.ScreenHeight),
					new Vector2i(Game.ScreenWidth, Game.ScreenHeight),
				    new Vector2i(Game.Width, Game.Height)) { 
				CaptureLayer = Layer.Normal,
				Layer = Layer.FX,
				Priority = Priority.Front
			};
			fx_buffer.SetHUD(_game.Camera);
			fx_buffer.SetFading(0.5f, new Colour(0,0,0,1), new Colour(0,0,0,0));
			m_house.AddDrawable(fx_buffer);
			m_house.AddUpdateable(fx_buffer);
			
		}
		
		public override void Update (float _delta)
		{
			if (m_keyboard.IsKeyPressed(Sdl.SDLK_SPACE) || m_keyboard.IsMouseButtonPressed(Sdl.SDL_BUTTON_LEFT)) {
				fx_buffer.SetFading(0.5f, new Colour(0,0,0,0), new Colour(0,0,0,1));
				endTimer = 0;
			}
			
			if (endTimer >= 0 && endTimer < 1) {
				endTimer += _delta;
			} else if (endTimer >= 1) {
				Exit(new ScreenMessage(){Message = "Welcome to the Magnum House..."});
			}
				
		}
	}
}
