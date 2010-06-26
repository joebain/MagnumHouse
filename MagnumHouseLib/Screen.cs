
using System;
using System.Drawing;
using System.Linq;

using Tao.Sdl;

namespace MagnumHouse
{
	public abstract class Screen
	{
		protected ObjectHouse m_house = new ObjectHouse();
		public ObjectHouse House { get { return m_house; } }
		protected UserInput m_keyboard;
		protected Game m_game;
		
		public event Action<Message> Exiting;
		
		Message m_message;
		
		public virtual void Setup(Game _game, UserInput _keyboard, Message _message) {
			m_message = _message;
			m_game = _game;
			m_keyboard = _keyboard;
		}
		
		public virtual void Update(float _delta) {
			
		}
		
		protected void Exit(Message _message) {
			if (Exiting != null) Exiting(_message);
		}
	}
	
	public class TitleScreen : Screen {
		
		public override void Setup (Game _game, UserInput _keyboard, Message _message)
		{
			base.Setup(_game, _keyboard, _message);
			
			Sprite title = new Sprite(new Bitmap("pictures/title.png"));
			title.Position = new Vector2f();
			title.Size = new Vector2f(Game.Width, Game.Height);
			
			m_house.AddDrawable(title);
			
			Text text = new Text("[Click] or Press [Space] to Start");
			text.CentreOn(new Vector2f(Game.Width/2, Game.Height/5));
			m_house.AddDrawable(text);
		}
		
		public override void Update (float _delta)
		{
			if (m_keyboard.IsKeyPressed(Sdl.SDLK_SPACE) || m_keyboard.IsMouseButtonPressed(Sdl.SDL_BUTTON_LEFT)) {
				Exit(new Message());
			}
		}
	}
	
	public class TargetLevel : Screen {
		
		protected Gangster gangsterNo1;
		float timePassed;
		
		public override void Setup (Game _game, UserInput _keyboard, Message _message)
		{
			base.Setup(_game, _keyboard, _message);
			
			TileMap tilemap = new TileMap("pictures/targetlevel.png");
			m_house = new ObjectHouse(tilemap);
			
			Background bg = new Background(tilemap);
			m_house.AddDrawable(bg);
			
			Magnum g1magnum = new Magnum(m_house);
			
			gangsterNo1 = new Hero(m_keyboard, g1magnum, tilemap, _game);
			gangsterNo1.Position = new Vector2f(1f, 10f);
			m_house.AddDrawable(gangsterNo1);
			m_house.AddUpdateable(gangsterNo1);
			
			m_house.AddDrawable(g1magnum);
			m_house.AddUpdateable(g1magnum);
			
			tilemap.Create(m_house, _game);
			
			Text score = new Text("Left: 00");
			score.updateAction = (_d) => {
				score.ChangeText("Left: " + m_house.GetAllDrawable<Target>().Count().ToString("00"));
			};
			score.SetHUD(_game);
			score.TopRight();
			
			m_house.AddUpdateable(score);
			m_house.AddDrawable(score);
			
			Text time = new Text("Time: 00:00");
			time.SetHUD(_game);
			time.TopLeft();
			timePassed = 0;
			time.updateAction = (_d) => {
				timePassed += _d;
				time.ChangeText("Time: " + timePassed.ToString("00.00").Replace(".",":"));
			};
			
			m_house.AddUpdateable(time);
			m_house.AddDrawable(time);
			
			
		}
		
		public override void Update (float _delta)
		{
			if (!m_house.GetAllDrawable<Target>().Any()) Exit(new Message() {Time = timePassed});
			if (m_keyboard.IsKeyPressed(Sdl.SDLK_s)) Exit(new Message() { Time = 69});
		}
	}
	
	public class EndScreen : Screen {
		
		public override void Setup (Game _game, UserInput _keyboard, Message _message)
		{
			base.Setup (_game, _keyboard, _message);
			
			Text message = new Text("Congratulations, you won!");
			message.SetHUD(_game);
			message.CentreOn(new Vector2f(Game.Width/2, 2*Game.Height/3));
			
			Text score = new Text("Time: " + _message.Time.ToString("00.00").Replace(".",":"));
			score.SetHUD(_game);
			score.CentreOn(new Vector2f(Game.Width/2, Game.Height/3));
			
			m_house.AddDrawable(message);
			m_house.AddDrawable(score);
			m_house.ProcessLists();
		}

	}
}
