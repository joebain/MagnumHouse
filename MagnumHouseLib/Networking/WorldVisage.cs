
using System;
using System.Collections.Generic;

using MagnumHouse;

namespace MagnumHouseLib
{
	public class WorldVisage
	{
		private List<GangsterVisage> gangsters = new List<GangsterVisage>();
		
		public event Action<GangsterVisage> GangsterAdded;
		
		public void AddGangster (GangsterVisage _gangster) {
			gangsters.Add(_gangster);
			GangsterAdded.Raise(_gangster);
		}
	}
}
