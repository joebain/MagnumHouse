
using System;

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
}
