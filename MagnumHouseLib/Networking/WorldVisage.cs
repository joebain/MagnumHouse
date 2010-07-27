
using System;
using System.Collections.Generic;

using MagnumHouseLib;

namespace MagnumHouseLib
{
	public class WorldVisage
	{
		private List<GangsterVisage> gangsters = new List<GangsterVisage>();
		
		public event Action<GangsterVisage> GangsterAdded;
		public event Action<GangsterVisage> GangsterRemoved;
		
		public void AddGangster (GangsterVisage _gangster) {
			gangsters.Add(_gangster);
			GangsterAdded.Raise(_gangster);
		}
		
		public void RemoveGangster (GangsterVisage _gangster) {
			gangsters.Remove(_gangster);
			GangsterRemoved.Raise(_gangster);
		}
		
		public int Count() {
			return gangsters.Count;
		}
	}
}
