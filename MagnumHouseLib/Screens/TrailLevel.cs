
using System;
using System.Drawing;

namespace MagnumHouseLib
{
	public class TrailLevel : Screen
	{
		public override void Setup (Game _game, UserInput _keyboard, ScreenMessage _message)
		{
			base.Setup (_game, _keyboard, _message);
			
			TileMap tilemap = new TileMap("pictures/empty_level.png");
			m_house = new ObjectHouse(tilemap);
			
			StarryBackground bg = new StarryBackground(tilemap.Size);
			m_house.AddDrawable(bg);
			
			Gangster gangsterNo1 = new Hero(m_keyboard, m_house);
			gangsterNo1.Position = new Vector2f(1f, 10f);
			gangsterNo1.PlaceInWorld(tilemap);
			m_house.AddDrawable(gangsterNo1);
			m_house.AddUpdateable(gangsterNo1);
			m_house.Add<IShootable>(gangsterNo1);
			m_game.SetCameraSubject(gangsterNo1);
			
			tilemap.Create(m_house, _game);
			
			Bitmap world_bmp = new Bitmap(tilemap.Width * Tile.Size, tilemap.Height * Tile.Size);
			for (int i = 0 ; i < world_bmp.Width ; i++) {
				for (int j = 0 ; j < world_bmp.Height; j++) {
					world_bmp.SetPixel(i, j, Color.Green);
				}
			}
			GangsterTrail trail = new GangsterTrail(gangsterNo1, new Sprite(world_bmp));
			m_house.AddUpdateable(trail);
			m_house.AddDrawable(trail);
		}

	}
}
