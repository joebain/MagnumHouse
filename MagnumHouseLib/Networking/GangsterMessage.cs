
using System;
using MagnumHouse;

namespace MagnumHouseLib
{
	public class GangsterMessage : INetworkMessage
	{
		public Vector2f Position = new Vector2f();
		public int Size { get { return byteSize; }}
		public const int byteSize = 10;
		
		public GangsterMessage() {
		}
		
		public GangsterMessage(byte[] _bytes) {
			FromBytes(_bytes);
		}
		
		public GangsterMessage (Vector2f _pos)
		{
			Position = _pos;
		}
		
		public void FromBytes(byte[] _bytes) {
			Position.X = System.BitConverter.ToSingle(_bytes, 2);
			Position.Y = System.BitConverter.ToSingle(_bytes, 6);
		}
		
		public byte[] GetBytes() {
			byte[] bytes = new byte[Size];
			System.BitConverter.GetBytes('g').CopyTo(bytes, 0);
			System.BitConverter.GetBytes((float)Position.X).CopyTo(bytes, 2);
			System.BitConverter.GetBytes((float)Position.Y).CopyTo(bytes, 6);
			return bytes;
		}
		
		public static bool SIs(byte[] _bytes) {
			var code = System.BitConverter.ToChar(_bytes, 0);
			Console.WriteLine("read char " + code);
			return code == 'g';
		}
		
		public bool Is(byte[] _bytes) {
			return SIs(_bytes);
		}
	}
}
