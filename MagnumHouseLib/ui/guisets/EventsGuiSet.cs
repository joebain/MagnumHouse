
using System;
using Tao.Sdl;

namespace MagnumHouseLib
{
	public class EventsGuiSet : GuiSet
	{
		Button timeEventButton;
		Button spaceEventButton;
		
		Button exitAddEventButton;
		Text label;
		
		public EventsGuiSet (Game _game, UserInput _keyboard, EditorLevel _editor) :
			base (_game, _keyboard, _editor)
		{
			exitAddEventButton = NewButton("Back", 0, ExitAddEvent);
			
			label = NewLabel("What kind of trigger?", 5);
			
			timeEventButton = NewButton("Timed", 2, MakeTimedEvent);
			spaceEventButton = NewButton("Spatial", 3, MakeSpaceEvent);
			
			Console.WriteLine("there are {0} events", m_editor.Map.Data.Events.Count);
			foreach (EventDescription e in m_editor.Map.Data.Events) {
				Console.WriteLine("event desc, {0}", e.GetType());
				if (e.trigger is SpatialTriggerDescription) {
					var se = (SpatialTriggerDescription) e.trigger;
					Console.WriteLine("trigger with box {0}", se.boxOn);
					NewBox(se.boxOn, new Colour(0.0f,1,0.5f, 0.5f));
				}
			}
			
		}
		
		private void ExitAddEvent() {
			m_editor.ChangeGuiSet(new StandardGuiSet(m_game, m_keyboard, m_editor));
		}
		
		private void MakeSpaceEvent() {
			m_editor.ChangeGuiSet(new SpatialEventGuiSet(m_game, m_keyboard, m_editor));
		}
		
		private void MakeTimedEvent() {
			m_editor.ChangeGuiSet(new TimedEventGuiSet(m_game, m_keyboard, m_editor));
		}
	}
}
