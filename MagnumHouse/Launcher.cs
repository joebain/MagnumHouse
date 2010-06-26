
using System;
using System.Threading;
using Gtk;

namespace MagnumHouse
{


	public class Launcher
	{

		static PhysicsAdjuster physicsAdjuster;
		static Game game;
		
		public static void Main() 
       	{
			game = new Game();
			
			var thread = new Thread(() => {
				Application.Init();
				physicsAdjuster = new PhysicsAdjuster();
				physicsAdjuster.Show();
				physicsAdjuster.DeleteEvent += (a, b) => {
					game.Quit();
				};
				Application.Run();
			});
			thread.Start();
			
			game.Setup();
			AttachAdjuster();
			game.Quitting += () => Application.Quit();
			game.Run();
			
			thread.Join();
		}
		
		public static void AttachAdjuster() {
			while (physicsAdjuster == null) Thread.Sleep(10);
			//physicsAdjuster.ParticularGangster = gangsterNo1;
			physicsAdjuster.ParticularGame = game;	
		}
	}
}
