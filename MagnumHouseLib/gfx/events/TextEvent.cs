
using System;

namespace MagnumHouseLib
{
	public class TextEvent : Event
	{
		Text m_text;
		ObjectHouse m_house;
		
		public TextEvent (Trigger trigger, Text text, ObjectHouse house) : base(trigger)
		{
			m_text = text;
			m_house = house;
		}
		
		public override void Start()
		{
			m_house.AddDrawable(m_text);
		}
		
		public override void Stop() {
			m_text.Die();
		}
	}
}
