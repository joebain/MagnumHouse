using System;
namespace MagnumHouseLib
{
	[Serializable]
	public class BoxDescription
	{
		public BoundingBox box;
		public String name;
		
		public BoxDescription() {
		}
		
		public BoxDescription (String name, BoundingBox box)
		{
			this.box = box;
			this.name = name;
		}
	}
}

