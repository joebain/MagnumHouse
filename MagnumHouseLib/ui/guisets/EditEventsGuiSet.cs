
using System;
using System.Collections.Generic;
using Tao.Sdl;

namespace MagnumHouseLib
{
	public class EditEventsGuiSet : GuiSet
	{
		Button timeEventButton;
		Button spaceEventButton;
		Button deleteButton;
		Button exitAddEventButton;
		Text label;
		
		Dictionary<ResizeableBox, EventDescription> eventBoxes = new Dictionary<ResizeableBox, EventDescription>();
		
		public EditEventsGuiSet (Game _game, UserInput _keyboard, EditorLevel _editor) :
			base (_game, _keyboard, _editor)
		{
			exitAddEventButton = NewButton("Back", 0, ExitAddEvent);
			
			label = NewLabel("Add:", 6);
			
			timeEventButton = NewButton("Timed", 4, MakeEvent<TimedTriggerDescription>);
			spaceEventButton = NewButton("Spatial", 5, MakeEvent<SpatialTriggerDescription>);
			
			deleteButton = NewButton("Delete", 2, DeleteEvent);
			
			Console.WriteLine("there are {0} events", m_editor.Map.Data.Events.Count);
			foreach (EventDescription e in m_editor.Map.Data.Events) {
				Console.WriteLine("event desc, {0}", e.GetType());
				if (e.trigger is SpatialTriggerDescription) {
					var se = (SpatialTriggerDescription) e.trigger;
					Console.WriteLine("trigger with box {0}", se.boxOn);
					var box = NewBox(se.boxOn, new Colour(0.0f,1,0.5f, 0.5f));
					box.MoveAction = _bb => {
						se.boxOn = _bb;
					};
					box.ResizeAction = _bb => {
						se.boxOn = _bb;	
					};
					eventBoxes[box] = e;
				}
			}
			
		}
		
		private void ExitAddEvent() {
			m_editor.ChangeGuiSet(new StandardGuiSet(m_game, m_keyboard, m_editor));
		}
		
		private void MakeEvent<T>() where T : TriggerDescription, new() {
			m_editor.ChangeGuiSet
				(new GeneralGuiSet<T>
				 (m_game, m_keyboard, m_editor,
				  _trigger => {
					m_editor.ChangeGuiSet(new ChooseEffectGuiSet(m_game, m_keyboard, m_editor, _trigger));
				},
				() => {
					m_editor.ChangeGuiSet(new EditEventsGuiSet(m_game, m_keyboard, m_editor));
				}));
		}
		
		private void DeleteEvent() {
			
			object focused = GuiItem.Focused;
			if (focused is ResizeableBox) {
				ResizeableBox eventBox = (ResizeableBox) focused;
				Console.WriteLine("there are {0} events", eventBoxes.Count);
				EventDescription eventDescription = eventBoxes[eventBox];
				eventBoxes.Remove(eventBox);
				m_editor.Map.Data.Events.Remove(eventDescription);
				eventBox.Die();
			}
		}
	}
}
