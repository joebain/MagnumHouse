
using System;
using System.Net.Sockets;

namespace MagnumHouseLib
{


	public class NetworkUtil
	{
		
		public const int port = 13000;

//		private static Socket ConnectSocket(string server, int port)
//	    {
//	        Socket s = null;
//	        IPHostEntry hostEntry = null;
//	
//	        // Get host related information.
//	        hostEntry = Dns.GetHostEntry(server);
//	
//	        // Loop through the AddressList to obtain the supported AddressFamily. This is to avoid
//	        // an exception that occurs when the host IP Address is not compatible with the address family
//	        // (typical in the IPv6 case).
//	        foreach(IPAddress address in hostEntry.AddressList)
//	        {
//	            IPEndPoint ipe = new IPEndPoint(address, port);
//	            Socket tempSocket = 
//	                new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
//	
//	            tempSocket.Connect(ipe);
//	
//	            if(tempSocket.Connected)
//	            {
//	                s = tempSocket;
//	                break;
//	            }
//	            else
//	            {
//	                continue;
//	            }
//	        }
//	        return s;
//	    }
	}
}
