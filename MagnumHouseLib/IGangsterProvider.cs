
using System;
using System.Collections.Generic;

namespace MagnumHouseLib
{
	public interface IGangsterProvider
	{
		IEnumerable<Gangster> GetAllGangsters();
	}
}
