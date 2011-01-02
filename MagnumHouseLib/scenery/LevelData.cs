
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MagnumHouseLib
{
	[Serializable]
	public class LevelData
	{
//		public int width, height;
//		
//		[XmlIgnore]
		public int[,] Map;
		
//		[XmlElement("Map")]
//		public int[] FlatMap
//		{
//		    get {
//				var map = new int[Width*Height];
//				for (int i = 0 ; i < Height ; i++) {
//					for (int j = 0 ; j < Width ; j++) {
//						map[i*Width+j] = Map[j, i];
//					}
//				}
//				return map;
//			}
//		    set {
//				Map = new int[height, width];
//				for (int i = 0 ; i < height ; i++) {
//					for (int j = 0 ; j < width ; j++) {
//						Map[i,j] = value[i*width+j];
//					}
//				}
//			}
//		}

		
		public int Width { get { return Map.GetLength(1); }}
		public int Height {get { return Map.GetLength(0); }}
		
//		public LevelData() {
//			Map = new int[width,height];
//		}
		
		public LevelData (int width, int height)
		{
			Map = new int[height, width];
		}
		
		public LevelData (int [,] map) {
			Map = map;
		}
		
		public int Get(int x, int y) {
			return Map[y, x];
		}
		
		public void Set(int x, int y, int val) {
			Map[y,x] = val;
		}
	}
}
