using System;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

namespace MagnumHouseLib
{
	
	public class TileMap : IDrawable
	{
		public Layer Layer { get { return Layer.Normal;}}
		private Priority m_priority = Priority.Middle;
		public Priority Priority { get { return m_priority;} set { m_priority = value; }}
		
		public bool Dead {get { return m_dead;}}
		
		private bool m_dead = false;
		public void Die() {m_dead = true;}
		
		public const int EMPTY = 0;
		public const int BLOCK = 1;
		public const int TARGET = 3;
		public const int PHONY = 2;
		public const int FLOOR = 4;
		public const int SPIKY = 5;
		
		public int Width { get { return Map.Width; }}
		public int Height {get { return Map.Height; }}
		public Vector2i Size {get { return new Vector2i(Width, Height); }}
		
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
		
		
		public LevelData Map;
		
		public LocationData locationData = new LocationData();
		
		public TileMap(Vector2i size) {
			Map = new LevelData(size.Y, size.X);
		}
		
		public TileMap(int[,] _map) {
			Map = new LevelData(_map);
		}
		
		public TileMap(string mapFile) {
			bool successLoading = false;
			
			successLoading = LoadFromPicture(mapFile);
			
			if (!successLoading) {
				successLoading = Load(mapFile);
			}
				
			if (!successLoading) {
				Console.WriteLine("File couldnt be opened: " + mapFile);
			}
			
		}
		
		bool LoadFromPicture(string mapFile) {
			
			try {
				Bitmap bmp = new Bitmap(mapFile);
				BitmapToMapData(bmp);
				return true;
			} catch {
				return false;
			}
		}
		
		void BitmapToMapData(Bitmap bmp) {
			Map = new LevelData(bmp.Width, bmp.Height);
			for (int x = 0 ; x < bmp.Width ; x++) {
				for (int y = 0 ; y < bmp.Height ; y++) {
					var colour = bmp.GetPixel(x, y);
					if (colour.R == 255) {
						if (colour.B == 255)
							Map.Set(x,y, FLOOR);
						else if (colour.G == 255)
							Map.Set(x,y, SPIKY);
						else
							Map.Set(x,y, BLOCK);
					}
					else if (colour.G == 255)
						Map.Set(x, y, PHONY);
					else if (colour.B == 255)
						Map.Set (x, y, TARGET);
					else
						Map.Set(x, y, EMPTY);
				}
			}	
		}
		
		private const string LOCATIONS_EXTENSION = ".locations.xml";
		private const string MAP_EXTENSION = ".map";

		private void WithNewFile(String filename, Action<FileStream> action) {
			if (File.Exists(filename)) {
				File.Delete(filename);
			}
			var file = File.OpenWrite(filename);
			action(file);
			file.Flush();
			file.Close();
		}
		
		public void Save(string filename) {
			
			var xf = new XmlSerializer(typeof(LocationData));
			WithNewFile(filename + LOCATIONS_EXTENSION, _file => xf.Serialize(_file, locationData));
			
			var bf = new BinaryFormatter();
			WithNewFile(filename + MAP_EXTENSION, _file => bf.Serialize(_file, Map));
		}
		
		public bool Load(string filename) {
			bool success = false;
			try {
				using (var file = File.OpenRead(filename + LOCATIONS_EXTENSION)) {	
					var bf = new XmlSerializer(typeof(LocationData));
					locationData = (LocationData)bf.Deserialize(file);
				}
				using (var file = File.OpenRead(filename + MAP_EXTENSION)) {	
					var bf = new BinaryFormatter();
					Map = (LevelData)bf.Deserialize(file);
				}
				
				success = true;
			}
			catch {
				Console.WriteLine("couldnt load file {0}", filename);
				success = false;
			}
			return success;
		}
		
		public void SetTileAt(Vector2i pos, int block) {
			int y = (Height-1)-pos.Y;
			if (pos.X < 0 || pos.X >= Width || y < 0 || y >= Height)
				return;
			Map.Set(pos.X, y, block);
		}
		
		public void Create (ObjectHouse _house, Game _game)
		{
			for (int x = 0 ; x < Width ; x++) {
				for (int y = 0 ; y < Height ; y++) {
					if (Map.Get(x,y) == BLOCK) {
						//_house.AddDrawable(new Tile(new Vector2i(x,(Height-1) - y)));
					} else if (Map.Get(x,y) == PHONY) {
						EasyPhony phony = new EasyPhony(_house, _house);
						phony.PlaceInWorld(this);
						phony.Position = new Vector2f(x, (Height-1) - y);
						_house.AddDrawable(phony);
						_house.AddUpdateable(phony);
						_house.Add<IShootable>(phony);
					} else if (Map.Get(x, y) == TARGET) {
						Target target = new Target();
						target.Position = new Vector2f(x, (Height-1)-y);
						_house.AddDrawable(target);
						_house.Add<IShootable>(target);
					} else if (Map.Get(x, y) == FLOOR) {
						//_house.AddDrawable(new FloorTile(new Vector2i(x, (Height-1)-y)));
					} else if (Map.Get(x,y) == SPIKY) {
						//_house.AddDrawable(new SpikyTile(new Vector2i(x, (Height-1)-y)));
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
		
		public LevelData GetMap() {
			return Map;
		}
		
		public int GetMap(int _x, int _y) {
			if (_x >= Width) return 0;
			else if (_x < 0) return 0;
			if (_y >= Height) return 0;
			else if (_y < 0) return 0;
			
			return Map.Get(_x, Height-(_y+1));
		}
		
		public void Draw() {
			for (int x = 0 ; x < Width ; x++) {
				for (int y = 0 ; y < Height ; y++) {
					DrawTile(Map.Get(x,y), Map.GetDepth(x,y,0), Map.GetDepth(x,y,1), new Vector2i(x, (Height-1)-y));
				}
			}
		}
		
		private void DrawTile(int tile, float depth1, float depth2, Vector2i pos) {
			switch (tile) {
			case BLOCK:
				Tile.Draw(pos);
				break;
			case FLOOR:
				FloorTile.Draw(pos);
				break;
			case SPIKY:
				SpikyTile.Draw(pos);
				break;
			}
		}
	}
}
