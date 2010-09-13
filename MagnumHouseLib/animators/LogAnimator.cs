
using System;

namespace MagnumHouseLib
{
	public class LogAnimator : IAnimator
	{
		float m_speed;
		float m_time;
		float m_position;
		
		public LogAnimator(float speed) {
			m_speed = speed;
		}
		
		public float Position {get{return m_position;}}
		
		public float Step(float delta) {
			m_time += m_speed*delta;
			
			m_position = (float)Math.Log10(m_time*9+1);
			return m_position;
		}
	}
}
