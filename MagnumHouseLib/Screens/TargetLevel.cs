
using System;
using System.Drawing;
using System.Linq;
using Tao.Sdl;

namespace MagnumHouseLib
{
	public class TargetLevel : Screen {
		
		protected Gangster gangsterNo1;
		float timePassed;
		
		public override void Setup (Game _game, UserInput _keyboard, ScreenMessage _message)
		{
			base.Setup(_game, _keyboard, _message);
			
			TileMap tilemap = new TileMap("pictures/targetlevel.png");
			m_house = new ObjectHouse(tilemap);
			
			StarryBackground bg = new StarryBackground(tilemap.Size);
			m_house.AddDrawable(bg);
			
			gangsterNo1 = new Hero(m_keyboard, m_house);
			gangsterNo1.Position = new Vector2f(1f, 10f);
			gangsterNo1.PlaceInWorld(tilemap);
			m_house.AddDrawable(gangsterNo1);
			m_house.AddUpdateable(gangsterNo1);
			m_game.SetCameraSubject(gangsterNo1);
			
			tilemap.Create(m_house, _game);
			
			Text score = new Text("Left: 00");
			score.updateAction = (_d) => {
				score.Contents = "Left: " + m_house.GetAllDrawable<Target>().Count().ToString("00");
			};
			score.SetHUD(_game.Camera);
			score.TopRight();
			
			m_house.AddUpdateable(score);
			m_house.AddDrawable(score);
			
			Text time = new Text("Time: 00:00");
			time.SetHUD(_game.Camera);
			time.TopLeft();
			timePassed = 0;
			time.updateAction = (_d) => {
				timePassed += _d;
				time.Contents = "Time: " + timePassed.ToString("00.00").Replace(".",":");
			};
			
			m_house.AddUpdateable(time);
			m_house.AddDrawable(time);
		}
		
		public override void Update (float _delta)
		{
			if (!m_house.GetAllDrawable<Target>().Any()) Exit(new ScreenMessage() {Time = timePassed});
			if (m_keyboard.IsKeyPressed(Sdl.SDLK_s)) Exit(new ScreenMessage() { Time = 69});
		}
	}
}
