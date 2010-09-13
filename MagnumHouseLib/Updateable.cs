
using System;

namespace MagnumHouseLib
{
	public class Updateable : IUpdateable
	{
		public bool Dead{get{return m_dead;}}
		bool m_dead = false;
		public int Id{get{return 0;}}

		public event Action<float> Updating;
		
		public Updateable ()
		{
		}
		
		public void Die() {
			m_dead = true;
		}
		
		public void Update(float _delta) {
			if (Updating != null) {
				Updating(_delta);
			}
		}
	}
}
