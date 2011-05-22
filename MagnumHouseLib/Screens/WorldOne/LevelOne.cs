using System;
using System.Drawing;
using System.Linq;
using Tao.Sdl;

namespace MagnumHouseLib
{
	public class LevelOne : Screen
	{
		protected Gangster gangsterNo1;
		
		public override Thing2D Character {get { return gangsterNo1;}}
		
		string levelFile = "level1";
				
		TileMap m_tilemap;
		
		float deathTimer;
		float startTimer;
		float winTimer;
		
		private Effect pixellyEffect;
		private Effect blurry_fx_buffer;
		private Effect death_fx_buffer;
		private Effect death_fx_buffer2;
		private Effect fadingEffect;
		
		Text welcome_message;
		BoundingBox endzone;
		
		public LevelOne ()
		{
		}
		
		public override void Setup (Game _game, UserInput _keyboard, ScreenMessage _message)
		{
			base.Setup(_game, _keyboard, _message);
			
			deathTimer = 0;
			startTimer = 0;
			winTimer = 0;
			
			m_tilemap = new TileMap(levelFile);
			
			m_size = new Vector2i(m_tilemap.Width, m_tilemap.Height);
			Bounds.Right = m_size.X;
			Bounds.Top = m_size.Y;
			m_house = new ObjectHouse(m_tilemap);
			
			StarryBackground bg = new StarryBackground(m_tilemap.Size);
			m_house.AddDrawable(bg);
			
			gangsterNo1 = new Hero(m_keyboard, m_house);
			gangsterNo1.Position = m_tilemap.locationData.start.point;
			gangsterNo1.PlaceInWorld(m_tilemap);
			m_house.AddDrawable(gangsterNo1);
			m_house.AddUpdateable(gangsterNo1);
			m_house.Add<IShootable>(gangsterNo1);
			m_game.SetCameraSubject(gangsterNo1);
			
			m_tilemap.Create(m_house, _game);
			m_house.AddDrawable(m_tilemap);
			
			endzone = m_tilemap.locationData.end.box;
			
			// fx
			
			// pixelly
			pixellyEffect
				= new Effect(
				    new Vector2i(Game.SmallScreenWidth/2, Game.SmallScreenHeight/2),
					new Vector2i(Game.ScreenWidth, Game.ScreenHeight),
				    new Vector2i(Game.Width, Game.Height)){ 
				CaptureLayer = Layer.Pixelly,
				Layer = Layer.FX,
				Priority = Priority.Front,
				Scaling = Sprite.ScaleType.Pixelly
			};
			fadingEffect.SetHUD(_game.Camera);
			m_house.AddDrawable(pixellyEffect);
			m_house.AddUpdateable(pixellyEffect);
			m_house.Add<IGrabing>(pixellyEffect);
			
			// fading
			fadingEffect
				= new Effect(
				    new Vector2i(Game.SmallScreenWidth/2, Game.SmallScreenHeight/2),
					new Vector2i(Game.ScreenWidth, Game.ScreenHeight),
				    new Vector2i(Game.Width, Game.Height)){ 
				CaptureLayer = Layer.All,
				Layer = Layer.FX,
				Priority = Priority.Front,
				Scaling = Sprite.ScaleType.Pixelly
			};

			fadingEffect.SetHUD(_game.Camera);
			fadingEffect.SetFading(1f, new Colour(0,0,0,1), new Colour(0,0,0,0));
//			fadingEffect.SetPixelling(new InverseLogAnimator(0.5f), 
//			                             new Vector2i(2, 2),
//			                             fadingEffect.CaptureSize);
			fadingEffect.SetBackground(new Colour(0,0,0,1f));
			m_house.AddDrawable(fadingEffect);
			m_house.AddUpdateable(fadingEffect);
			m_house.Add<IGrabing>(fadingEffect);
			
			
			// messages
			welcome_message = new Text("Welcome to the Magnum House...");
			welcome_message.SetHUD(m_game.Camera);
			welcome_message.CentreOn(Game.Size.ToF()/2);
			welcome_message.Priority = Priority.Front;
			welcome_message.Layer = Layer.FX;
			welcome_message.Transparency = 0f;
			m_house.AddDrawable(welcome_message);
		}
		
		public override void Update (float _delta)
		{
			base.Update (_delta);
			
			// end zone
			if (endzone.Overlaps(gangsterNo1.Bounds)) {
				winTimer = 1;
				fadingEffect.SetBackground(new Colour(0,0,0,0));
				fadingEffect.SetFading(1f, new Colour(0,0,0,0), new Colour(0,0,0,1));
				fadingEffect.SetPixelling(new InverseLogAnimator(0.5f), 
				                             new Vector2i(2, 2),
				                             fadingEffect.CaptureSize);
				m_house.AddDrawable(fadingEffect);
				m_house.AddUpdateable(fadingEffect);
				m_house.Add<IGrabing>(fadingEffect);
			}
			if (winTimer > 0 && winTimer <= 2) {
				winTimer += _delta;	
			} else if (winTimer > 2) {
				Exit(new ScreenMessage(){Message = "See you again soon."});
			}
			if (startTimer <= 2) {
				if (startTimer <= 1) welcome_message.Transparency += _delta;
				startTimer += _delta;
			} else if (startTimer <= 3) {
				if (!fadingEffect.Dead) fadingEffect.Die();
				welcome_message.Transparency -= _delta;
				startTimer += _delta;
			} else {
				welcome_message.Die();
			}
		}
	}
}

