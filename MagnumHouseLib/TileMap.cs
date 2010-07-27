using System;
using System.Drawing;

namespace MagnumHouseLib
{
	public class TileMap
	{
		public int Width { get { return Map.GetLength(1); }}
		public int Height {get { return Map.GetLength(0); }}
		
		public static readonly int[,] Level1 = new int[,] {
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0},
			{0,0,0,0,0,0,0,0,0,0,1,0,2,0,0,0,0,1,0,0},
			{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
		};
		
		private int[,] Map;
		
		public TileMap(int[,] _map) {
			Map = _map;
		}
		
		public TileMap(string mapFile) {
			Bitmap bmp = new Bitmap(mapFile);
			Map = new int[bmp.Height, bmp.Width];
			for (int x = 0 ; x < bmp.Width ; x++) {
				for (int y = 0 ; y < bmp.Height ; y++) {
					var colour = bmp.GetPixel(x, y);
					if (colour.R == 255)
						Map[y,x] = 1;
					else if (colour.G == 255)
						Map[y,x] = 2;
					else if (colour.B == 255)
						Map[y,x] = 3;
					else
						Map[y,x] = 0;
				}
			}
		}
		
		public void Create (ObjectHouse _house, Game _game)
		{
			for (int x = 0 ; x < Width ; x++) {
				for (int y = 0 ; y < Height ; y++) {
					if (Map[y,x] == 1) {
						_house.AddDrawable(new Tile(new Vector2i(x,(Height-1) - y)));
					} else if (Map[y,x] == 2) {
						Magnum magnum = new Magnum(_house);
						_house.AddDrawable(magnum);
						_house.AddUpdateable(magnum);
						Phony phony = new Phony(magnum, this, _game, _house);
						phony.Position = new Vector2f(x, (Height-1) - y);
						_house.AddDrawable(phony);
						_house.AddUpdateable(phony);
						_house.Add<IShootable>(phony);
					} else if (Map[y,x] == 3) {
						Target target = new Target();
						target.Position = new Vector2f(x, (Height-1)-y);
						_house.AddDrawable(target);
						_house.Add<IShootable>(target);
					}
				}
			}
			_house.ProcessLists();
		}
		
		public TileProximity GetProximity(Vector2f _pos) {
			
			TileProximity prox = new TileProximity();
			Vector2i gridPos = _pos.Floor();
			Vector2f offset = _pos - gridPos.ToF();
			int left = (int) Math.Round(offset.X);
			int right = (int) Math.Round(1-offset.X);
			int above = (int) Math.Round(offset.Y);
			int below = (int) Math.Round(1-offset.Y);
			if (GetMap(gridPos.X+left,gridPos.Y+above) == 1 ||
			    GetMap(gridPos.X+left,gridPos.Y+above) == 1 ||
			    GetMap(gridPos.X+right,gridPos.Y+below) == 1 ||
			    GetMap(gridPos.X+right,gridPos.Y+below) == 1) {
				prox.On = 1;
			}
			if (GetMap(gridPos.X+left, gridPos.Y+below) == 1 ||
			    GetMap(gridPos.X+right, gridPos.Y+below) == 1) {
				prox.Below = 1;
			}
			if (GetMap(gridPos.X+left, gridPos.Y+above) == 1 ||
			    GetMap(gridPos.X+right, gridPos.Y+above) == 1) {
				prox.Above = 1;
			}
			if (GetMap(gridPos.X+left, gridPos.Y+above) == 1 ||
			    GetMap(gridPos.X+left, gridPos.Y+below) == 1) {
				prox.Left = 1;
			}
			if (GetMap(gridPos.X+right, gridPos.Y+above) == 1 ||
			    GetMap(gridPos.X+right, gridPos.Y+below) == 1) {
				prox.Right = 1;
			}
			
			return prox;
		}
		
		public bool IsCollision(Vector2f _pos, Vector2f _size) {
			return
			    GetMap((int)(_pos.X), (int)(_pos.Y)) == 1 ||
			    GetMap((int)(_pos.X), (int)(_pos.Y + _size.Y)) == 1 ||
			    GetMap((int)(_pos.X + _size.X), (int)(_pos.Y)) == 1 ||
			    GetMap((int)(_pos.X + _size.X), (int)(_pos.Y + _size.Y)) == 1;
		}
		
		public int GetMap(int _x, int _y) {
			if (_x >= Width) return 0;
			else if (_x < 0) return 0;
			if (_y >= Height) return 0;
			else if (_y < 0) return 0;
			
			return Map[Height-(_y+1),_x];
		}
	}
}
