
using System;
using MagnumHouseLib;

namespace MagnumHouseLib
{
	public class GangsterMessage : INetworkMessage
	{
		public Vector2f Position = new Vector2f();
		public System.Int32 Id;
		public int Size { get { return byteSize; }}
		public const int byteSize = 14;
		
		public GangsterMessage(byte[] _bytes) {
			FromBytes(_bytes);
		}
		
		public GangsterMessage (Int32 _id, Vector2f _pos)
		{
			Id = _id;
			Position = _pos;
		}
		
		public void FromBytes(byte[] _bytes) {
			Id = System.BitConverter.ToInt32(_bytes, 2);
			Position.X = System.BitConverter.ToSingle(_bytes, 6);
			Position.Y = System.BitConverter.ToSingle(_bytes, 10);
		}
		
		public byte[] GetBytes() {
			byte[] bytes = new byte[Size];
			System.BitConverter.GetBytes('g').CopyTo(bytes, 0);
			System.BitConverter.GetBytes(Id).CopyTo(bytes, 2);
			System.BitConverter.GetBytes((float)Position.X).CopyTo(bytes, 6);
			System.BitConverter.GetBytes((float)Position.Y).CopyTo(bytes, 10);
			return bytes;
		}
		
		public static bool SIs(byte[] _bytes) {
			var code = System.BitConverter.ToChar(_bytes, 0);
			//Console.WriteLine("read char " + code);
			return code == 'g';
		}
		
		public bool Is(byte[] _bytes) {
			return SIs(_bytes);
		}
	}
}
