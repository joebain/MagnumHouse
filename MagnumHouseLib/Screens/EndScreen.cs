
using System;
using System.Drawing;
using System.Linq;
using Tao.Sdl;
namespace MagnumHouseLib
{
public class EndScreen : Screen {
		
		public override void Setup (Game _game, UserInput _keyboard, ScreenMessage _message)
		{
			base.Setup (_game, _keyboard, _message);
			
			Text message = new Text(_message.Message);
			message.SetHUD(_game.Camera);
			message.CentreOn(new Vector2f(Game.Width/2, 2*Game.Height/3));
			
//			Text score = new Text("Time: " + _message.Time.ToString("00.00").Replace(".",":"));
//			score.SetHUD(_game.Camera);
//			score.CentreOn(new Vector2f(Game.Width/2, Game.Height/3));
			
			m_house.AddDrawable(message);
//			m_house.AddDrawable(score);
			m_house.ProcessLists();
		}

	}
}
