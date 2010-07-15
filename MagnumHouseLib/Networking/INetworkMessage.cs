
using System;

namespace MagnumHouseLib
{
	public interface INetworkMessage
	{
		int Size {get;}
		byte[] GetBytes();
		void FromBytes(byte[] _bytes);
		
		bool Is(byte[] _bytes);
	}
}
