
using System;

namespace MagnumHouseLib
{
	public class ClientHelloMessage : INetworkMessage
	{
		public ClientHelloMessage ()
		{
		}
		
		public ClientHelloMessage(Int32 id) {
			Id = id;
		}
		
		public ClientHelloMessage(byte[] _bytes) {
			FromBytes(_bytes);
		}
		
		const int byteSize = 6;
		public int Size {get {return byteSize;}}
		
		public System.Int32 Id;
		
		public byte[] GetBytes() {
			byte[] bytes = new byte[byteSize];
			System.BitConverter.GetBytes('h').CopyTo(bytes, 0);
			System.BitConverter.GetBytes(Id).CopyTo(bytes, 2);
			return bytes;
		}
		
		public void FromBytes(byte[] _bytes) {
			Id = System.BitConverter.ToInt32(_bytes,2);
		}
		
		public bool Is(byte[] _bytes) {
			return SIs(_bytes);
		}
		
		public static bool SIs(byte[] _bytes) {
			return System.BitConverter.ToChar(_bytes, 0) == 'h';
		}
	}
}
