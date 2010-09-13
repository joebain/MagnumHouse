
using System;
using Tao.Sdl;

namespace MagnumHouseLib
{


	public class TextEffectGuiSet : OkCancelGuiSet
	{
		TextBox newText;
		
		TriggerDescription Trigger;
		Updateable updateable;
		
		public TextEffectGuiSet (Game _game, UserInput _keyboard, EditorLevel _editor, TriggerDescription trigger) :
			base (_game, _keyboard, _editor)
		{
			Trigger = trigger;
			
			MakeTextEvent();
			newText.Position = Game.ScreenCentre/2 + _game.Camera.ViewOffset;
		}
		
		private void MakeTextEvent() {
			newText = new TextBox(m_keyboard, new Vector2f(10, 2), null);
			newText.backgroundColour = new Colour(0, 0.3f, 1, 1);
			updateable = new Updateable();
			updateable.Updating += _delta => {
				if (newText.Bounds.Contains(m_keyboard.MousePos) &&
				    m_keyboard.IsMouseButtonPressed(Sdl.SDL_BUTTON_LEFT)
				) {
					newText.CentreOn(m_keyboard.MousePos);
				}
			};
			items.Add(newText);
			m_editor.House.AddUpdateable(updateable);
		}
		
		public override void Ok ()
		{
			updateable.Die();
			var ted = new TextEventDescription();
			ted.pos = newText.Position;
			ted.text = newText.Text;
			ted.trigger = Trigger;
			
			m_editor.Map.Data.Events.Add(ted);
			
			m_editor.ChangeGuiSet(new StandardGuiSet(m_game, m_keyboard, m_editor));
		}
		
		public override void Cancel ()
		{
			updateable.Die();
			m_editor.ChangeGuiSet(new ChooseEffectGuiSet(m_game, m_keyboard, m_editor, Trigger));
		}
	}
}
