
using System;

namespace MagnumHouseLib
{
	public class MessageCodes
	{
		public const char CLIENT_HELLO = 'H';
		public const char HELLO = 'h';
		public const char GANGSTER = 'g';
		public const char CLIENT_GOODBYE = 'B';
		public const char GOODBYE = 'b';
		public const char SLUG = 's';
		
		public static Type GetType(byte[] bytes) {
			char code = System.BitConverter.ToChar(bytes, 0);
			switch (code) {
			case CLIENT_HELLO:
				return typeof(ClientHelloMessageContent);
			case CLIENT_GOODBYE:
				return typeof(ClientGoodbyeMessageContent);
			case GANGSTER:
				return typeof(GangsterMessageContent);
			case SLUG:
				return typeof(SlugMessageContent);
			case GOODBYE:
				return typeof(GoodbyeMessageContent);
			case HELLO:
				return typeof(HelloMessageContent);
			}
			return null;
		}
		
		public static char FromType(Type t) {
			if (t == typeof(ClientHelloMessageContent))
				return CLIENT_HELLO;
			else if (t == typeof(ClientGoodbyeMessageContent))
				return CLIENT_GOODBYE;
			else if (t == typeof(GangsterMessageContent))
				return GANGSTER;
			else if (t == typeof(SlugMessageContent))
				return SLUG;
			else if (t == typeof(GoodbyeMessageContent))
				return GOODBYE;
			else if (t == typeof(HelloMessageContent))
				return HELLO;
			else
				return '\0';
		}
	}
}
