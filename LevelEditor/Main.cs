
using System;
using MagnumHouseLib;

namespace LevelEditor
{
	public class Launcher
	{
		static Game game;
		
		public static void Main ()
		{
			game = new Game();
			game.SetLevels(new [] {new EditorLevel()});
			game.Setup();
			game.Run();
		}
	}
}
