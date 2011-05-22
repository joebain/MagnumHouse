
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
		public float[,] DepthMap;
		
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
			DepthMap = new int[height, width*2];
		}
		
		public LevelData (int [,] map) {
			Map = map;
		}
		
		public int Get(int x, int y) {
			return Map[y, x];
		}
		
		public float GetDepth(int x, int y, int half) {
			return DepthMap[y, x*2 + half];
		}
		
		public void Set(int x, int y, int val) {
			Map[y,x] = val;
		}
		
		public void setDepth(int x, int y, int half, float val) {
			DepthMap[y,x*2+half] = val;
	}
}
