
using System;

namespace MagnumHouseLib
{


	public class SpatialEventGuiSet : OkCancelGuiSet
	{

		ResizeableBox box;
		
		public SpatialEventGuiSet (Game _game, UserInput _keyboard, EditorLevel _editor) :
			base (_game, _keyboard, _editor)
		{
			box = NewBox(_game.Camera.ViewOffset + new Vector2f(5,5), new Vector2f(5,5), new Colour(0,0.8f,0.5f,0.5f));
		}
		
		public override void Ok ()
		{
			var description = new SpatialTriggerDescription();
			description.boxOn = box.Bounds;
			m_editor.ChangeGuiSet(new ChooseEffectGuiSet(m_game, m_keyboard, m_editor, description));
		}
		
		public override void Cancel ()
		{
			m_editor.ChangeGuiSet(new EventsGuiSet(m_game, m_keyboard, m_editor));
		}
	}
}
