
using System;
using System.Collections.Generic;

namespace MagnumHouseLib
{
	public class NetworkHouse
	{
		Dictionary<Type, Dictionary<Int32, IUpdateable>> stuff = new Dictionary<Type, Dictionary<Int32, IUpdateable>>();
		
		public void AddUpdateable(IUpdateable _updateable) {
			if (_updateable is Hero) {
				Hero hero = (Hero)_updateable;
				//stuff[typof(Hero)][]
			}
		}
	}
}
