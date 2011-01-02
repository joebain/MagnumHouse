
using System;

namespace MagnumHouseLib
{
	public class InverseLogAnimator : IAnimator
	{
		float m_speed;
		float m_time;
		float m_position;
		
		public InverseLogAnimator(float speed) {
			m_speed = speed;
		}
		
		public float Position {get{return m_position;}}
		
		public float Step(float delta) {
			m_time += m_speed*delta;
			
			m_position = 1 - (float)Math.Log10((1-m_time)*9+1);
			return m_position;
		}
	}
}
