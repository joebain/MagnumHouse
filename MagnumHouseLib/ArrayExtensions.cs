
using System;
using System.Collections.Generic;

namespace MagnumHouseLib
{
	public static class ArrayExtensions
	{
		public static void CopySub(this byte[] _source, byte[] _dest, int _sourceOffset, int _destOffset, int length) {
			for (int i = 0 ; i < length; i++) {
				_dest[i+_destOffset] = _source[i+_sourceOffset];
			}
		}
	}
	
	public static class IEnumerableExtensions {
		public static void ForEach<T>(this IEnumerable<T> list, Action<T> action) {
			foreach( var e in list) {
				action(e);
			}
		}
	}
}
