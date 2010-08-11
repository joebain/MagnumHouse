using System;

using MagnumHouseLib;

namespace MagnumHouseLib
{
	public class NetworkLevel : Screen
	{
		public override void Setup (Game _game, UserInput _keyboard, ScreenMessage _message)
		{
			base.Setup (_game, _keyboard, _message);
			
			var map = new TileMap("pictures/testlevel.png");
			m_house = new ObjectHouse(map);
			
			Background bg = new Background(map);
			m_house.AddDrawable(bg);
			
			map.Create(m_house, _game);
			
			
			Magnum magnum = new Magnum(m_house);
			
			var hero = new Hero(_keyboard, m_house);
			hero.Position = new Vector2f(1f, 10f);
			hero.PlaceInWorld(map);
			m_house.AddDrawable(hero);
			m_house.AddUpdateable(hero);
			
			m_house.AddDrawable(magnum);
			m_house.AddUpdateable(magnum);
			
			var nHero = new NetworkHero(hero, "127.0.0.1", m_house);
			m_house.AddUpdateable(nHero);
			nHero.Connect();
		}
	}
}
