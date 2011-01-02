
using System;

namespace MagnumHouseLib
{
	public class ChooseEffectGuiSet : GuiSet
	{
		Button textEventButton;
		Button fxEventButton;
		Button soundEventButton;
		Text label;
		TriggerDescription Trigger;
		
		public ChooseEffectGuiSet (Game _game, UserInput _keyboard, EditorLevel _editor, TriggerDescription trigger) :
			base (_game, _keyboard, _editor)
		{
			Trigger = trigger;
			
			label = NewLabel("What kind of event?", 6);
			textEventButton = NewButton("Text", 2, MakeTextEvent);
			fxEventButton = NewButton("FX", 3, MakeFXEvent);
			soundEventButton = NewButton("Sound", 4, MakeSoundEvent);
		}
		
		public void MakeTextEvent() {
			m_editor.ChangeGuiSet(new TextEffectGuiSet(m_game, m_keyboard, m_editor, Trigger));
		}
		
		public void MakeFXEvent() {
			m_editor.ChangeGuiSet(new EventsGuiSet(m_game, m_keyboard, m_editor));
		}
		
		public void MakeSoundEvent() {
			m_editor.ChangeGuiSet(new EventsGuiSet(m_game, m_keyboard, m_editor));
		}
	}
}
