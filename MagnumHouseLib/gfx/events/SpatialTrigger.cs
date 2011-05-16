
using System;

namespace MagnumHouseLib
{
	public class SpatialTrigger : Trigger
	{
		public BoundingBox m_onBox;
		public BoundingBox m_offBox;
		
		bool m_useBoxOff;
		
		Thing2D Trigger;
		
		public SpatialTrigger (BoundingBox box, Thing2D trigger) : this(box, null, false, trigger)
		{
		}
		
		public SpatialTrigger (BoundingBox on, BoundingBox off, bool useBoxOff, Thing2D trigger) {
			m_onBox = on;
			m_offBox = off;
			m_useBoxOff = useBoxOff;
			Trigger = trigger;
		}
		
		public override bool ConditionOff ()
		{
			if ((m_offBox == null || !m_useBoxOff) && !m_onBox.Overlaps(Trigger.Bounds)) {
				return true;
			}
			if ((m_offBox != null && m_useBoxOff) && m_offBox.Overlaps(Trigger.Bounds)) {
				return true;
			}	
			return false;
		}

		public override bool ConditionOn() {
			return m_onBox.Overlaps(Trigger.Bounds);
		}
	}
}
