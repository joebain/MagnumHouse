using System;

namespace MagnumHouse
{
	public class RangeAttribute : Attribute
	{
		public float m_min;
		public float m_max;
		public int m_places;
		
		public RangeAttribute (float min, float max, int places)
		{
			m_min = min;
			m_max = max;
			m_places = places;
		}
	}
}
