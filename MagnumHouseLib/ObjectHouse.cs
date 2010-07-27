
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

using Tao.OpenGl;

namespace MagnumHouseLib
{
	public class ObjectHouse
	{
		public static bool drawBoundingBoxes;
		
		Dictionary<Type, SyncList<Object>> m_things = new Dictionary<Type, SyncList<Object>>();
		
		TileMap m_map;
		
		private NetworkHouse NetworkHouse;
		List<Type> interceptedTypes = new List<Type>();
		
		
		public ObjectHouse () {
			m_things[typeof(IDrawable)] = new SyncList<object>();
			m_things[typeof(IUpdateable)] = new SyncList<object>();
		}
		
		public ObjectHouse (TileMap _map)
		{
			m_map = _map;
			
			m_things[typeof(IDrawable)] = new SyncList<object>();
			m_things[typeof(IUpdateable)] = new SyncList<object>();
		}
		public void Add<T>(T _addee) {
			SyncList<Object> list;
			if (m_things.TryGetValue(typeof(T), out list)) {
				list.Add(_addee);
			} else {
				list = new SyncList<Object>();
				list.Add(_addee);
				m_things.Add(typeof(T), list);
			}
		}
		
		public void Remove<T>(T _removee) {
			SyncList<Object> list;
			if (m_things.TryGetValue(typeof(T), out list)) {
				list.Remove(_removee);
			}
		}
		
		public void SetNetworkHouse(NetworkHouse house) {
			NetworkHouse = house;
		}
		
		public void RegisterNetworkType(Type intercept) {
			interceptedTypes.Add(intercept);
		}
		
		public void AddUpdateable(IUpdateable _updateable) {
			if (interceptedTypes.Contains(_updateable.GetType())) {
				NetworkHouse.AddUdateable(_updateable);
			} else {
				Add<IUpdateable>(_updateable);
			}
		}
		
		public void RemoveUpdateable(IUpdateable _updateable) {
			Remove<IUpdateable>(_updateable);
		}
		
		public void AddDrawable(IDrawable _drawable) {
			Add<IDrawable>(_drawable);
		}
		
		public void RemoveDrawable(IDrawable _drawable) {
			Remove<IDrawable>(_drawable);
		}
		
		private SyncList<object> GetList<T>() {
			return m_things[typeof(T)];
		}
		
		public void Draw() {
			GetList<IDrawable>().NiftyFor<IDrawable>(_d => _d.Draw(), _d => _d.Dead);
			
			if (drawBoundingBoxes) {
				foreach (var thing in GetAllDrawable<IThing2D>()) {
					Gl.glPushMatrix();
					Gl.glTranslatef(thing.Position.X, thing.Position.Y, 0.1f);
					Gl.glColor3f(0,1,0);
					Gl.glBegin(Gl.GL_LINE_LOOP);
					Gl.glVertex2f(0,0);
					Gl.glVertex2f(0, thing.Size.Y);
					Gl.glVertex2f(thing.Size.X, thing.Size.Y);
					Gl.glVertex2f(thing.Size.X, 0);
					Gl.glEnd();
					Gl.glPopMatrix();
				}
			}
		}
		
		public void Update(float _delta) {
			GetList<IUpdateable>().NiftyFor<IUpdateable>(_u => _u.Update(_delta), _u => _u.Dead);
			
			UpdateSlugs();
		}
		
		private void UpdateSlugs() {
			foreach (var slug in GetAllSlugs()) {
				if (m_map.IsCollision(slug.Position, slug.Size)) {
					slug.HitSomething(ThingsToHit.Wall, null);
				} else if (slug.Position.X > m_map.Width || slug.Position.X < 0
				           || slug.Position.Y > m_map.Height || slug.Position.Y < 0) {
					slug.HitSomething(ThingsToHit.OOB, null);
				} else {
					GetList<IShootable>().NiftyFor<IShootable>(
					_gangster =>
					{
						if (_gangster.Bounds.Overlaps(slug.Bounds)) {
							slug.HitSomething(ThingsToHit.Gangster, _gangster);
							_gangster.GotShot(slug);
							return true;
						}
						return false;
					},
					_g => _g.Dead
					);
					
				}
				
			}
		}
		
		public IEnumerable<Slug> GetAllSlugs() {
			return GetAllUpdateable<Slug>();
		}
		
		public IEnumerable<Gangster> GetAllGangsters() {
			return GetAllUpdateable<Gangster>();
		}
		
		public IEnumerable<Phony> GetAllPhonies() {
			return GetAllUpdateable<Phony>();
		}
		
		public IEnumerable<T> GetAllUpdateable<T>() {
			return GetAll<T>(GetList<IUpdateable>());
		}
		
		public IEnumerable<T> GetAll<T>(IEnumerable _enum) {
			foreach (var updateable in _enum) {
				if (updateable is T) {
					yield return (T) updateable;
				}
			}
		}
		
		public IEnumerable<T> GetAllDrawable<T>() {
			return GetAll<T>(GetList<IDrawable>());
		}
		
		public void ProcessLists() {
			foreach (var thing in m_things) {
				thing.Value.Process();
			}
		}
		
		public void Die() {
			GetList<IUpdateable>().NiftyFor<IUpdateable>(_u => _u.Die(), _u => false);
		}
	}
}
