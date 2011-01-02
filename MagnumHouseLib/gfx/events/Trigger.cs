
using System;

namespace MagnumHouseLib
{
	public abstract class Trigger : IUpdateable
	{
		bool m_dead = false;
		public bool Dead {get {return m_dead;}}
		public int Id{get{return 0;}}
		
		public bool OneShot = true;
		
		bool m_hasBeenTriggered = false;
		
		public void Die() {
			m_dead = true;
		}
		
		public event Action TriggerOn;
		public event Action TriggerOff;
		
		public virtual void Update(float _delta) {
			if (!m_hasBeenTriggered && ConditionOn()) {
				m_hasBeenTriggered = true;
				if (TriggerOn != null) {
					TriggerOn();
				}
			}
			else if (m_hasBeenTriggered && ConditionOff()) {
				if (!OneShot) m_hasBeenTriggered = false;
				if (TriggerOff != null) {
					TriggerOff();
				}
			}
		}
		
		public abstract bool ConditionOn();
		public abstract bool ConditionOff();
	}
}
