using System;
namespace MagnumHouseLib
{
	[Serializable]
	public class PointDescription
	{
		public Vector2f point;
		public String name;
		
		public PointDescription() {
		}
		
		public PointDescription (String name, Vector2f point)
		{
			this.name = name;
			this.point = point;
		}
	}
}

