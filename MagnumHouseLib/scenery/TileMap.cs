using System;
using System.Drawing;
using System.IO;

namespace MagnumHouseLib
{
	public class TileMap
	{
		public const int EMPTY = 0;
		public const int BLOCK = 1;
		public const int TARGET = 3;
		public const int PHONY = 2;
		public const int FLOOR = 4;
		public const int SPIKY = 5;
		
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
			Bitmap bmp;
			try {
				bmp = new Bitmap(mapFile);
			} catch {
				Console.WriteLine("File couldnt be opened: " + mapFile);
				throw;
			}
			Map = new int[bmp.Height, bmp.Width];
			for (int x = 0 ; x < bmp.Width ; x++) {
				for (int y = 0 ; y < bmp.Height ; y++) {
					var colour = bmp.GetPixel(x, y);
					if (colour.R == 255) {
						if (colour.B == 255)
							Map[y,x] = FLOOR;
						else if (colour.G == 255)
							Map[y,x] = SPIKY;
						else
							Map[y,x] = BLOCK;
					}
					else if (colour.G == 255)
						Map[y,x] = PHONY;
					else if (colour.B == 255)
						Map[y,x] = TARGET;
					else
						Map[y,x] = EMPTY;
				}
			}
		}
		
		public void Create (ObjectHouse _house, Game _game)
		{
			for (int x = 0 ; x < Width ; x++) {
				for (int y = 0 ; y < Height ; y++) {
					if (Map[y,x] == BLOCK) {
						_house.AddDrawable(new Tile(new Vector2i(x,(Height-1) - y)));
					} else if (Map[y,x] == PHONY) {
						EasyPhony phony = new EasyPhony(_house, _house);
						phony.PlaceInWorld(this);
						phony.Position = new Vector2f(x, (Height-1) - y);
						_house.AddDrawable(phony);
						_house.AddUpdateable(phony);
						_house.Add<IShootable>(phony);
					} else if (Map[y,x] == TARGET) {
						Target target = new Target();
						target.Position = new Vector2f(x, (Height-1)-y);
						_house.AddDrawable(target);
						_house.Add<IShootable>(target);
					} else if (Map[y,x] == FLOOR) {
						_house.AddDrawable(new FloorTile(new Vector2i(x, (Height-1)-y)));
					} else if (Map[y,x] == SPIKY) {
						_house.AddDrawable(new SpikyTile(new Vector2i(x, (Height-1)-y)));
					}
				}
			}
			_house.ProcessLists();
		}
		
		public int IsCollision(Vector2f _pos, Vector2f _size) {
			int maxX = (int)Math.Ceiling(_pos.X + _size.X);
			int maxY = (int)Math.Ceiling(_pos.Y + _size.Y);
			int minY = (int)Math.Floor(_pos.Y);
			int minX = (int)Math.Floor(_pos.X);
			int height = maxY - minY;
			int width = maxX - minX;
			int[,] collisions = new int[width, height];
			bool anyBlockCollisions = false;
			for (int x = minX ; x < maxX ; x++) {
				for (int y = minY ; y < maxY ; y++) {
					collisions[x-minX,y-minY] = GetMap(x,y);
					if (collisions[x-minX,y-minY] == BLOCK) {
						anyBlockCollisions = true;
						break;
					}
				}
			}
			
			if (anyBlockCollisions) return BLOCK;
			
			bool floorCollision = false;
			for (int x = 0 ; x < width; x++) {
				if (collisions[x,0] == FLOOR) {
					floorCollision = true;
					break;
				}
			}
			
			if (floorCollision) return FLOOR;
			
			bool spikeCollision = false;
			for (int x = 0; x < width ; x++) {
				if (collisions[x,0] == SPIKY) {
					spikeCollision = true;
					break;
				}
			}
			
			if (spikeCollision) return SPIKY;
			
			return EMPTY;
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
