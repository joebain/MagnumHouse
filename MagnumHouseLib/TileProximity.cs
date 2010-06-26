using System;

namespace MagnumHouse
{
	public class TileProximity
	{
		public Vector2f GridOffset;
		public float RightClear;
		public float LeftClear;
		public float UpClear;
		public float DownClear;
		public int Above;
		public int Left;
		public int Right;
		public int Below;
		public int On;
		
		public void Print() {
			Console.WriteLine(String.Format("----------\n|#{0}#|\n|{1}{2}{3}|\n|#{4}#|\n----------", Above, Left, On, Right, Below));
			Console.WriteLine(String.Format("Right : {0}, Left {1}, Up {2}, Down {3}",RightClear,LeftClear, UpClear, DownClear));
			Console.WriteLine("tile: " + GridOffset);
		}
		
		public override string ToString ()
		{
			return String.Format("----------\n|#{0}#|\n|{1}{2}{3}|\n|#{4}#|\n----------", Above, Left, On, Right, Below);
		}

	}
}
