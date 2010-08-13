
using System;
using System.Drawing;
using System.Linq;
using Tao.Sdl;

namespace MagnumHouseLib
{
	public class PlatformLevel: Screen
	{
		protected Gangster gangsterNo1;
		
		string LevelPic;
		
		public PlatformLevel(string levelPic) {
			LevelPic = levelPic;
		}
		
		public override void Setup (Game _game, UserInput _keyboard, ScreenMessage _message)
		{
			base.Setup(_game, _keyboard, _message);
			
			TileMap tilemap = new TileMap(LevelPic);
			m_house = new ObjectHouse(tilemap);
			
			Background bg = new Background(tilemap);
			m_house.AddDrawable(bg);
			
			gangsterNo1 = new Hero(m_keyboard, m_house);
			gangsterNo1.Position = new Vector2f(1f, 10f);
			gangsterNo1.PlaceInWorld(tilemap);
			m_house.AddDrawable(gangsterNo1);
			m_house.AddUpdateable(gangsterNo1);
			m_house.Add<IShootable>(gangsterNo1);
			m_game.SetCameraSubject(gangsterNo1);
			
			tilemap.Create(m_house, _game);
			
			Text score = new Text("Left: 00");
			score.updateAction = (_d) => {
				score.ChangeText("Left: " + m_house.GetAllDrawable<Target>().Count().ToString("00"));
			};
			score.SetHUD(_game);
			score.TopRight();
			
			m_house.AddUpdateable(score);
			m_house.AddDrawable(score);
			
			Score score_pic = new Score(gangsterNo1);
			score_pic.SetHUD(_game);
			score_pic.TopLeft();
			
			m_house.AddUpdateable(score_pic);
			m_house.AddDrawable(score_pic);
		}
		
		public override void Update (float _delta)
		{
			if (!m_house.GetAllDrawable<Target>().Any()) Exit(new ScreenMessage());
		}
	}
}
