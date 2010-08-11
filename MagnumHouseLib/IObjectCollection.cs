
using System;

namespace MagnumHouseLib
{
	public interface IObjectCollection
	{
		void Add<T>(T _addee);
		void Remove<T>(T _removee);
		void AddUpdateable(IUpdateable _updateable);
		void RemoveUpdateable(IUpdateable _updateable);
		void AddDrawable(IDrawable _drawable);
		void RemoveDrawable(IDrawable _drawable);
	}
}
