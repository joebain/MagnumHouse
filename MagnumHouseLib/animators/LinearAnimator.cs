
using System;

namespace MagnumHouseLib
{
	public class LinearAnimator : IAnimator
	{
		float m_time;
		float m_speed;
		
		public LinearAnimator(float speed) {
			m_speed = speed;
		}
		
		public float Position {get{return m_time;}}
		
		public float Step(float delta) {
			m_time += delta * m_speed;
			return m_time;
		}
	}
}
