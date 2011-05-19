using System;
using System.Collections.Generic;
using System.Xml.Serialization;
namespace MagnumHouseLib
{
	[Serializable]
	public class LocationData
	{
		public List<BoxDescription> boxes = new List<BoxDescription>();
		public List<PointDescription> points = new List<PointDescription>();
		public PointDescription start = new PointDescription();
		public BoxDescription end = new BoxDescription();
	}
}

