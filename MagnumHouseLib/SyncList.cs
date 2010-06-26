using System;
using System.Collections;
using System.Collections.Generic;

namespace MagnumHouse
{
	public class SyncList<T> : IEnumerable<T>
	{
		private List<T> m_list = new List<T>();
		private Queue m_toRemove = new Queue();
		private Queue m_toAdd = new Queue();
		
		public void Add(T _item) {
			Queue.Synchronized(m_toAdd).Enqueue(_item);
		}
		
		public void Remove(T _item) {
			Queue.Synchronized(m_toRemove).Enqueue(_item);
		}
		
		public void Process() {
			SynchronizedDequeueOperation(m_toRemove, _item => m_list.Remove(_item));
			SynchronizedDequeueOperation(m_toAdd, _item => m_list.Add(_item));
		}
		
		private void SynchronizedDequeueOperation(Queue _queue, Action<T> _operation) {
			lock (_queue.SyncRoot) {
				while (_queue.Count != 0) {
					_operation((T)_queue.Dequeue());
				}
			}
		}
		
		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
		
		public IEnumerator<T> GetEnumerator() {
			return m_list.GetEnumerator();
		}
		
		public void NiftyFor<T2>(Action<T2> _action, Func<T2, bool> _remove) where T2 : class {
			NiftyFor<T2>(_thing => {_action(_thing); return false;}, _remove);
		}
		
		public void NiftyFor<T2>(Func<T2, bool> _action, Func<T2,bool> _remove) where T2 : class{
			
			foreach (var thing in m_list) {
				var castThing = thing as T2;
				if (castThing == null) continue;
				if (_remove(castThing))
					Remove(thing);
				if (_action(castThing))
					break;
			}
			Process();
		}
	}
}
