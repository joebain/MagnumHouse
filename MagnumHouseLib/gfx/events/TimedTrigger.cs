
using System;

namespace MagnumHouseLib
{
	public class TimedTrigger : Trigger
	{
		float m_time = 0;
		float m_duration;
		float m_on;

		public TimedTrigger(float on, float duration) {
			m_on = on;
			m_duration = duration;
		}
		
		public override void Update(float _delta) {
			m_time += _delta;
		}
		
		public override bool ConditionOff ()
		{
			return m_time > m_on + m_duration;
		}
		
		public override bool ConditionOn () {
			return m_time > m_on;
		}
	}
}
