
using System;
using System.Drawing;
using System.Linq;

using Tao.Sdl;

namespace MagnumHouseLib
{
	public abstract class Screen
	{
		protected ObjectHouse m_house = new ObjectHouse();
		public ObjectHouse House { get { return m_house; } }
		protected UserInput m_keyboard;
		protected Game m_game;
		
		public event Action<ScreenMessage> Exiting;
		
		ScreenMessage m_message;
		
		public virtual void Setup(Game _game, UserInput _keyboard, ScreenMessage _message) {
			m_message = _message;
			m_game = _game;
			m_keyboard = _keyboard;
		}
		
		public virtual void Update(float _delta) {
			
		}
		
		protected void Exit(ScreenMessage _message) {
			if (Exiting != null) Exiting(_message);
		}
	}
}
