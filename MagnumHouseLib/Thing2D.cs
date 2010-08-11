
using System;

namespace MagnumHouseLib
{
	public interface IThing2D {
		Vector2f Position {get;}
		Vector2f Size {get;}
		Vector2f Speed {get;}
		BoundingBox Bounds {get;}
		void SetHUD(Game _game);
	}
	
	public abstract class Thing2D : IThing2D, IDeadable
	{
		public virtual int Id {get; set;}
		
		private bool HUD;
		private Game m_game;
		private Vector2f m_position = new Vector2f();
		public virtual Vector2f Position {
			get { 
				if (!HUD)
					return m_position;
				else {
					return m_position - m_game.viewOffset;
				}
			} 
			set {
				//if (!HUD)
					m_position = value;
				//else
					//m_position = value + m_game.viewOffset;
			}}
		private Vector2f m_size = new Vector2f();
		public virtual Vector2f Size {get {return m_size; } set{m_size = value;}}
		private Vector2f m_speed = new Vector2f();
		public virtual Vector2f Speed {get{return m_speed;} set{m_speed = value;}}
		public BoundingBox Bounds {get {return new BoundingBox(Position.X, Position.Y, Position.X + Size.X, Position.Y + Size.Y);}}
		public virtual bool Dead {get; set;}
		
		public virtual void SetHUD(Game _game) {
			HUD = true;
			m_game = _game;
		}
		
		public void CentreOn(Vector2f _pos) {
			Position = new Vector2f(_pos.X - Size.X/2, _pos.Y + Size.Y/2);
		}
		
		public void TopRight() {
			Position = new Vector2f(Game.Width - Size.X, Game.Height - Size.Y);
		}
		
		public void TopLeft() {
			Position = new Vector2f(0, Game.Height - Size.Y);
		}
		
		public void BottomCentre() {
			Position = new Vector2f(Game.Width/2 - Size.X/2, 0);
		}
		
		public virtual void Die() {}
	}
}
