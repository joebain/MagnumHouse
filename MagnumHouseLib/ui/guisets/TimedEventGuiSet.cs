
using System;

namespace MagnumHouseLib
{
	public class TimedEventGuiSet : OkCancelGuiSet
	{
		TextBox timeBox;
		TextBox durationBox;
		
		public TimedEventGuiSet (Game _game, UserInput _keyboard, EditorLevel _editor) :
			base (_game, _keyboard, _editor)
		{
			timeBox = NewTextBox("Start time:","0.0", 5);
			durationBox = NewTextBox("Duration:","1.0", 3);
		}
		
		public override void Ok ()
		{
			var description = new TimedTriggerDescription();
			description.start = float.Parse(timeBox.Text);
			description.length = float.Parse(durationBox.Text);
			m_editor.ChangeGuiSet(new ChooseEffectGuiSet(m_game, m_keyboard, m_editor, description));
		}
		
		public override void Cancel ()
		{
			m_editor.ChangeGuiSet(new EventsGuiSet(m_game, m_keyboard, m_editor));
		}
	}
}
