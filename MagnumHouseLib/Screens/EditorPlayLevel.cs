
using System;

namespace MagnumHouseLib
{
	public class EditorPlayLevel : PlatformLevel
	{
		public EditorPlayLevel ()
		{
		}
		
		public override void Setup (Game _game, UserInput _keyboard, ScreenMessage _message)
		{
			base.Setup (_game, _keyboard, _message);
			
			gangsterNo1.Position = _message.Position;
			
			EditorCursor cursor = new EditorCursor(_keyboard, Bounds);
			m_house.AddUpdateable(cursor);
			Button button = new Button(m_keyboard, "Back", m_game.Camera) {
				Position = new Vector2f(1, 1)
			};
			button.Pressed += () => {
				Exit(new ScreenMessage(){
					Position = m_game.Camera.ViewOffset + Game.ScreenCentre,
					Level = _message.Level
				});
			};
			m_house.AddDrawable(button);
			m_house.AddUpdateable(button);
			
			m_game.AddLevel(new EditorLevel());
		}
	}
}
