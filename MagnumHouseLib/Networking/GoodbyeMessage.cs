
using System;

namespace MagnumHouseLib
{
	public class GoodbyeMessage : INetworkMessage
	{
		public Int32 Id;
		const int byteSize = 6;
		public int Size { get { return byteSize; } }
		
		public GoodbyeMessage (Int32 id)
		{
			Id = id;
		}
		
		public GoodbyeMessage (byte[] _bytes) {
			FromBytes(_bytes);
		}
		
		public void FromBytes(byte[] _bytes) {
			Id = System.BitConverter.ToInt32(_bytes, 2);
		}
		
		public byte[] GetBytes() {
			byte[] bytes = new byte[byteSize];
			System.BitConverter.GetBytes('b').CopyTo(bytes,0);
			System.BitConverter.GetBytes(Id).CopyTo(bytes,2);
			return bytes;
		}
		
		public static bool SIs(byte[] bytes) {
			return bytes[0] == 'b';
		}
		
		public bool Is(byte[] bytes) {
			return SIs(bytes);
		}
	}
}
