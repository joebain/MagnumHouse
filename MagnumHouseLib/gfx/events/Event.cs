
using System;

namespace MagnumHouseLib
{
	public abstract class Event : IUpdateable
	{
		bool m_dead = false;
		public bool Dead{get{return m_dead;}}
		public int Id{get{return 0;}}
		
		public Trigger m_trigger;

		public Event (Trigger trigger)
		{
			Contracts.Assert(() => {return trigger != null;});
			m_trigger = trigger;
			m_trigger.TriggerOn += Start;
			m_trigger.TriggerOff += Stop;
		}
		
		public void Die() {
			m_dead = true;
		}
		
		public virtual void Start() {
			
		}
		
		public virtual void Stop() {
			
		}
		
		public virtual void Update(float delta) {
			m_trigger.Update(delta);
		}
	}
}
