
using System;
using System.Drawing;
using System.Linq;
using Tao.Sdl;

namespace MagnumHouseLib
{
	public class PlatformLevel: Screen
	{
		protected Gangster gangsterNo1;
		
		string LevelPic;
				
		TileMap m_tilemap;
		
		float deathTimer;
		
		private ScreenSprite pixelly_fx_buffer;
		private ScreenSprite blurry_fx_buffer;
		private ScreenSprite death_fx_buffer;
			
		public PlatformLevel(string levelPic) {
			
			LevelPic = levelPic;
		}
		
		public override void Setup (Game _game, UserInput _keyboard, ScreenMessage _message)
		{
			base.Setup(_game, _keyboard, _message);
			
			deathTimer = 0;
			
			m_tilemap = new TileMap(LevelPic);
			m_size = new Vector2i(m_tilemap.Width, m_tilemap.Height);
			m_house = new ObjectHouse(m_tilemap);
			
			Background bg = new Background(m_tilemap);
			m_house.AddDrawable(bg);
			
			gangsterNo1 = new Hero(m_keyboard, m_house);
			gangsterNo1.Position = new Vector2f(1f, 10f);
			gangsterNo1.PlaceInWorld(m_tilemap);
			m_house.AddDrawable(gangsterNo1);
			m_house.AddUpdateable(gangsterNo1);
			m_house.Add<IShootable>(gangsterNo1);
			m_game.SetCameraSubject(gangsterNo1);
			
			m_tilemap.Create(m_house, _game);
			
			Text score = new Text("Left: 00");
			score.updateAction = (_d) => {
				score.ChangeText("Left: " + m_house.GetAllDrawable<Target>().Count().ToString("00"));
			};
			score.SetHUD(_game.Camera);
			score.TopRight();
			
			m_house.AddUpdateable(score);
			m_house.AddDrawable(score);
			
			Score score_pic = new Score(gangsterNo1);
			score_pic.SetHUD(_game.Camera);
			score_pic.TopLeft();
			
			m_house.AddUpdateable(score_pic);
			m_house.AddDrawable(score_pic);
			
			
			pixelly_fx_buffer
				= new ScreenSprite(
					new Vector2i(Game.SmallScreenWidth, Game.SmallScreenHeight),
					new Vector2i(Game.ScreenWidth, Game.ScreenHeight),
				    new Vector2i(Game.Width, Game.Height)) { 
				CaptureLayer = Layer.Pixelly,
				Scaling = Sprite.ScaleType.Pixelly,
				Feedback = 0.9f,
				Priority = Priority.Front
			};
			pixelly_fx_buffer.updateAction = () => {
				pixelly_fx_buffer.Position = -_game.Camera.LastOffset;
			};
			
			m_house.AddDrawable(pixelly_fx_buffer);
			m_house.AddUpdateable(pixelly_fx_buffer);
			m_house.Add<IGrabing>(pixelly_fx_buffer);
			
			blurry_fx_buffer
				= new ScreenSprite(
					new Vector2i(Game.SmallScreenWidth/2, Game.SmallScreenHeight/2),
					new Vector2i(Game.ScreenWidth, Game.ScreenHeight),
				    new Vector2i(Game.Width, Game.Height)) { 
				CaptureLayer = Layer.Blurry,
				Scaling = Sprite.ScaleType.Blurry,
				Feedback = 0.9f,
				Priority = Priority.Back
			};
			blurry_fx_buffer.SetHUD(_game.Camera);
//			blurry_fx_buffer.updateAction = () => {
//				blurry_fx_buffer.Position = Vector2f.Zero;
//			};
			
			m_house.AddDrawable(blurry_fx_buffer);
			m_house.AddUpdateable(blurry_fx_buffer);
			m_house.Add<IGrabing>(blurry_fx_buffer);
			
			death_fx_buffer
				= new ScreenSprite(
				    new Vector2i(Game.SmallScreenWidth/2, Game.SmallScreenHeight/2),
					new Vector2i(Game.ScreenWidth, Game.ScreenHeight),
				    new Vector2i(Game.Width, Game.Height)) { 
				CaptureLayer = Layer.Normal,
				Scaling = Sprite.ScaleType.Blurry,
				Feedback = 0.99f,
				Priority = Priority.Front
			};
			death_fx_buffer.SetHUD(_game.Camera);
			death_fx_buffer.updateAction = () => {
				death_fx_buffer.Position = Vector2f.Zero;
			};
			death_fx_buffer.SetSpinning(5);
			death_fx_buffer.SetZooming(-0.2f);

		}
		
		public override void Update (float _delta)
		{
			if (!m_house.GetAllDrawable<Target>().Any()) Exit(new ScreenMessage());
			if (gangsterNo1.Dead && deathTimer < 1) {
				deathTimer = 1;
				m_house.AddDrawable(death_fx_buffer);
				m_house.AddUpdateable(death_fx_buffer);
				m_house.Add<IGrabing>(death_fx_buffer);
			}
			if (deathTimer >= 1) {
				deathTimer += _delta;
				if (deathTimer >= 3) {
					Reload(new ScreenMessage());
				}
			}
		}
	}
}
