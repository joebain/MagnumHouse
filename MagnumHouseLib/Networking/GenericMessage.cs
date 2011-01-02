
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MagnumHouseLib
{
	public class GenericMessage
	{
		public Type ContentType;
		private char ContentCode;
		public MessageContent Content;
		
		public GenericMessage(Type t, MessageContent content) {
			ContentType = t;
			ContentCode = MessageCodes.FromType(t);
			Content = content;
		}
		
		public GenericMessage(byte[] bytes) {
			FromBytes(bytes);
		}
		
		public byte[] GetBytes() {
			BinaryFormatter formatter = new BinaryFormatter();
			byte[] bytes = new byte[100]; //may need to work this out
			formatter.Serialize(new MemoryStream(bytes, 0, 2), ContentCode);
			formatter.Serialize(new MemoryStream(bytes, 2, 98), Content);
			return bytes;
		}
		
		public void FromBytes(byte[] bytes) {
			if (bytes[0] == '0') {
				ContentType = typeof(GoodbyeMessageContent);
				return;
			}
			ContentType = MessageCodes.GetType(bytes);
			BinaryFormatter formatter = new BinaryFormatter();
			Content = (MessageContent) formatter.Deserialize(new MemoryStream(bytes, 2, bytes.Length-2));
		}
	}
}
