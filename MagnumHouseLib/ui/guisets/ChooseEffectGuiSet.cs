
using System;

namespace MagnumHouseLib
{
	public class ChooseEffectGuiSet : GuiSet
	{
		Button textEventButton;
		Button fadeEventButton;
		Button shakeEventButton;
		Button soundEventButton;
		Text label;
		TriggerDescription Trigger;
		
		public ChooseEffectGuiSet (Game _game, UserInput _keyboard, EditorLevel _editor, TriggerDescription trigger) :
			base (_game, _keyboard, _editor)
		{
			Trigger = trigger;
			
			label = NewLabel("What kind of event?", 6);
			textEventButton = NewButton("Text", 2, MakeEvent<TextEventDescription>);
			soundEventButton = NewButton("Sound", 3, MakeSoundEvent);
			fadeEventButton = NewButton("Fade", 4, MakeEvent<FadeEventDescription>);
			shakeEventButton = NewButton("Shake", 5, MakeEvent<ShakeEventDescription>);
		}
		
		public void MakeEvent<T>() where T : EventDescription, new() {
			GeneralGuiSet<T> guiSet
				= new GeneralGuiSet<T>
					(m_game, m_keyboard, m_editor,
					ted => {
						ted.trigger = Trigger;
						m_editor.Map.Data.Events.Add(ted);
						m_editor.ChangeGuiSet(new EditEventsGuiSet(m_game, m_keyboard, m_editor));
					},
					() => {
						m_editor.ChangeGuiSet(new EditEventsGuiSet(m_game, m_keyboard, m_editor));
					});
			m_editor.ChangeGuiSet(guiSet);
		}
		
		public void MakeSoundEvent() {
			m_editor.ChangeGuiSet(new EditEventsGuiSet(m_game, m_keyboard, m_editor));
		}
	}
}
