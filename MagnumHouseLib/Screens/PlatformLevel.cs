
using System;
using System.Drawing;
using System.Linq;
using Tao.Sdl;

namespace MagnumHouseLib
{
	public class PlatformLevel: Screen
	{
		protected Gangster gangsterNo1;
		
		public override Thing2D Character {get { return gangsterNo1;}}
		
		string levelFile;
				
		TileMap m_tilemap;
		
		float deathTimer;
		float startTimer;
		float winTimer;
		
		private Effect pixelly_fx_buffer;
		private Effect blurry_fx_buffer;
		private Effect death_fx_buffer;
		private Effect death_fx_buffer2;
		private Effect start_fx_buffer;
		
		Text welcome_message;
		
		public PlatformLevel() {
			
		}
		
		public PlatformLevel(string _levelFile) {
			levelFile = _levelFile;
		}
		
		public override void Setup (Game _game, UserInput _keyboard, ScreenMessage _message)
		{
			base.Setup(_game, _keyboard, _message);
			
			deathTimer = 0;
			startTimer = 0;
			winTimer = 0;
			
			if (_message.Level != null) {
				m_tilemap = _message.Level;
			} else {
				m_tilemap = new TileMap(levelFile);
			}
			
			m_size = new Vector2i(m_tilemap.Width, m_tilemap.Height);
			m_house = new ObjectHouse(m_tilemap);
			
			Background bg = new Background(m_tilemap.Size);
			m_house.AddDrawable(bg);
			
			gangsterNo1 = new Hero(m_keyboard, m_house);
			gangsterNo1.Position = new Vector2f(1f, 10f);
			gangsterNo1.PlaceInWorld(m_tilemap);
			m_house.AddDrawable(gangsterNo1);
			m_house.AddUpdateable(gangsterNo1);
			m_house.Add<IShootable>(gangsterNo1);
			m_game.SetCameraSubject(gangsterNo1);
			
			m_tilemap.Create(m_house, _game);
			m_house.AddDrawable(m_tilemap);
			
			m_events = m_tilemap.Data.Events.Select(_e => _e.MakeReal(this));
			m_events.ForEach(_e => m_house.AddUpdateable(_e));
			
//			Text score = new Text("Left: 00");
//			score.updateAction = (_d) => {
//				score.Contents = "Left: " + m_house.GetAllDrawable<Target>().Count().ToString("00");
//			};
//			score.SetHUD(_game.Camera);
//			score.TopRight();
//			
//			m_house.AddUpdateable(score);
//			m_house.AddDrawable(score);
//			
//			Score score_pic = new Score(gangsterNo1);
//			score_pic.SetHUD(_game.Camera);
//			score_pic.TopLeft();
//			
//			m_house.AddUpdateable(score_pic);
//			m_house.AddDrawable(score_pic);
//			
//			
//			pixelly_fx_buffer
//				= new ScreenSprite(
//					new Vector2i(Game.SmallScreenWidth, Game.SmallScreenHeight),
//					new Vector2i(Game.ScreenWidth, Game.ScreenHeight),
//				    new Vector2i(Game.Width, Game.Height)) { 
//				CaptureLayer = Layer.Pixelly,
//				Layer = Layer.Normal,
//				Scaling = Sprite.ScaleType.Pixelly,
//				Feedback = 0.9f,
//				Priority = Priority.Front
//			};
//			pixelly_fx_buffer.updateAction = () => {
//				pixelly_fx_buffer.Position = -_game.Camera.LastOffset;
//			};
//			
//			m_house.AddDrawable(pixelly_fx_buffer);
//			m_house.AddUpdateable(pixelly_fx_buffer);
//			m_house.Add<IGrabing>(pixelly_fx_buffer);
//			
//			blurry_fx_buffer
//				= new ScreenSprite(
//					new Vector2i(Game.SmallScreenWidth/2, Game.SmallScreenHeight/2),
//					new Vector2i(Game.ScreenWidth, Game.ScreenHeight),
//				    new Vector2i(Game.Width, Game.Height)) { 
//				CaptureLayer = Layer.Blurry,
//				Layer = Layer.Normal,
//				Scaling = Sprite.ScaleType.Blurry,
//				Feedback = 0.9f,
//				Priority = Priority.Back
//			};
//			blurry_fx_buffer.SetHUD(_game.Camera);
//			
//			m_house.AddDrawable(blurry_fx_buffer);
//			m_house.AddUpdateable(blurry_fx_buffer);
//			m_house.Add<IGrabing>(blurry_fx_buffer);
//			
//			death_fx_buffer
//				= new ScreenSprite(
//				    new Vector2i(Game.SmallScreenWidth/2, Game.SmallScreenHeight/2),
//					new Vector2i(Game.ScreenWidth, Game.ScreenHeight),
//				    new Vector2i(Game.Width, Game.Height)) { 
//				CaptureLayer = Layer.Normal,
//				Layer = Layer.FX,
//				Feedback = 0.99f,
//				Priority = Priority.Front
//			};
//			death_fx_buffer.SetHUD(_game.Camera);
//			death_fx_buffer.SetSpinning(15);
//			death_fx_buffer.SetZooming(-0.2f);
//			
//			
//			
//			
//			start_fx_buffer
//				= new ScreenSprite(
//				    new Vector2i(Game.SmallScreenWidth/2, Game.SmallScreenHeight/2),
//					new Vector2i(Game.ScreenWidth, Game.ScreenHeight),
//				    new Vector2i(Game.Width, Game.Height)) { 
//				CaptureLayer = Layer.Normal,
//				Layer = Layer.FX,
//				Priority = Priority.Front,
//				Scaling = Sprite.ScaleType.Pixelly
//			};
//			start_fx_buffer.SetHUD(_game.Camera);
//			start_fx_buffer.SetFading(1f, new float[]{0,0,0,1}, new float[]{0,0,0,0});
//			start_fx_buffer.SetPixelling(new InverseLogAnimator(0.5f), 
//			                             new Vector2i(2, 2),
//			                             start_fx_buffer.CaptureSize);
//			start_fx_buffer.SetBackground(new float[]{0,0,0,1f});
//			m_house.AddDrawable(start_fx_buffer);
//			m_house.AddUpdateable(start_fx_buffer);
//			m_house.Add<IGrabing>(start_fx_buffer);
//			
//			welcome_message = new Text(_message.Message);
//			welcome_message.SetHUD(m_game.Camera);
//			welcome_message.CentreOn(Game.Size.ToF()/2);
//			welcome_message.Priority = Priority.Front;
//			welcome_message.Layer = Layer.FX;
//			welcome_message.Transparency = 0f;
//			m_house.AddDrawable(welcome_message);

		}
		
		public override void Update (float _delta)
		{
//			if (!m_house.GetAllDrawable<Target>().Any()) {
//				winTimer = 1;
//				start_fx_buffer.SetBackground(new float[]{0,0,0,0});
//				start_fx_buffer.SetFading(1f, new float[]{0,0,0,0}, new float[]{0,0,0,1});
//				start_fx_buffer.SetPixelling(new InverseLogAnimator(0.5f), 
//				                             new Vector2i(2, 2),
//				                             start_fx_buffer.CaptureSize);
//				m_house.AddDrawable(start_fx_buffer);
//				m_house.AddUpdateable(start_fx_buffer);
//				m_house.Add<IGrabing>(start_fx_buffer);
//			}
//			if (winTimer > 0 && winTimer <= 2) {
//				winTimer += _delta;
				
//			} else if (winTimer > 2) {
//				Exit(new ScreenMessage(){Message = "See you again soon."});
//			}
//			if (startTimer <= 2) {
//				if (startTimer <= 1) welcome_message.Transparency += _delta;
//				startTimer += _delta;
//			} else if (startTimer <= 3) {
//				if (!start_fx_buffer.Dead) start_fx_buffer.Die();
//				welcome_message.Transparency -= _delta;
//				startTimer += _delta;
//			} else {
//				welcome_message.Die();
//			}
			
//			if (gangsterNo1.Dead && deathTimer < 1) {
//				deathTimer = 1;
//				
//				m_house.AddDrawable(death_fx_buffer);
//				m_house.AddUpdateable(death_fx_buffer);
//				m_house.Add<IGrabing>(death_fx_buffer);
//				
//				m_house.AddDrawable(death_fx_buffer2);
//				m_house.AddUpdateable(death_fx_buffer2);
//			}
//			if (deathTimer >= 1) {
//				deathTimer += _delta;
//				if (deathTimer >= 3) {
//					Reload(new ScreenMessage(){Message = "Welcome back..."});
//				}
//			}
		}
	}
}
