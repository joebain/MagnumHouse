using System;
using MagnumHouseLib;

namespace Tools
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Game game = new Game();
			
			game.Setup();
			game.Run();
		}
	}
}
