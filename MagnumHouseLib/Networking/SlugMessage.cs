using System;

using MagnumHouseLib;

namespace MagnumHouseLib
{
	public class SlugMessage : INetworkMessage
	{
		
		public Vector2f Position;
		public Vector2f Velocity;
		public Int32 G_id;
		
		public const int byteSize = 22;
		
		public int Size { get { return byteSize;}}
		
		public SlugMessage (Vector2f position, Vector2f velocity, Int32 g_id)
		{
			Position = position;
			Velocity = velocity;
			G_id = g_id;
		}
		
		public SlugMessage(byte[] _bytes) {
			FromBytes(_bytes);
		}
		
		public void FromBytes(byte[] _bytes) {
			Position.X = System.BitConverter.ToSingle(_bytes, 2);
			Position.Y = System.BitConverter.ToSingle(_bytes, 6);
			Velocity.X = System.BitConverter.ToSingle(_bytes, 10);
			Velocity.Y = System.BitConverter.ToSingle(_bytes, 14);
			G_id = System.BitConverter.ToInt32(_bytes, 18);
		}
		
		public byte[] GetBytes() {
			byte[] bytes = new byte[byteSize];
			System.BitConverter.GetBytes('s').CopyTo(bytes,0);
			System.BitConverter.GetBytes(Position.X).CopyTo(bytes,2);
			System.BitConverter.GetBytes(Position.Y).CopyTo(bytes,6);
			System.BitConverter.GetBytes(Velocity.X).CopyTo(bytes,10);
			System.BitConverter.GetBytes(Velocity.Y).CopyTo(bytes,14);
			System.BitConverter.GetBytes(G_id).CopyTo(bytes,18);
		}
		
		public bool Is(byte[] _bytes) {
			return SIs(_bytes);
		}
		
		public static bool SIs(byte[] _bytes) {
			return System.BitConverter.ToChar(_bytes, 0) == 's';
		}
	}
}
