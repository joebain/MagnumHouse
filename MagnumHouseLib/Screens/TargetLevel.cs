
using System;
using System.Drawing;
using System.Linq;
using Tao.Sdl;
using MagnumHouseLib;

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
			
			Background bg = new Background(tilemap);
			m_house.AddDrawable(bg);
			
			Magnum g1magnum = new Magnum(m_house);
			
			gangsterNo1 = new Hero(m_keyboard, g1magnum, tilemap, _game);
			gangsterNo1.Position = new Vector2f(1f, 10f);
			
			m_house.AddDrawable(gangsterNo1);
			m_house.AddUpdateable(gangsterNo1);
			
			m_house.AddDrawable(g1magnum);
			m_house.AddUpdateable(g1magnum);
			
			tilemap.Create(m_house, _game);
			
			Text score = new Text("Left: 00");
			score.updateAction = (_d) => {
				score.ChangeText("Left: " + m_house.GetAllDrawable<Target>().Count().ToString("00"));
			};
			score.SetHUD(_game);
			score.TopRight();
			
			m_house.AddUpdateable(score);
			m_house.AddDrawable(score);
			
			Text time = new Text("Time: 00:00");
			time.SetHUD(_game);
			time.TopLeft();
			timePassed = 0;
			time.updateAction = (_d) => {
				timePassed += _d;
				time.ChangeText("Time: " + timePassed.ToString("00.00").Replace(".",":"));
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
