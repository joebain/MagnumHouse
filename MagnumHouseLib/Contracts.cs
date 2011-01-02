
using System;

namespace MagnumHouseLib
{
	public class AssertionNotMetException : Exception {
		public AssertionNotMetException() {
			
		}
		
		public AssertionNotMetException(string message) : base(message) {
			
		}
	}
	
	public class Contracts
	{
		public static void Assert(Func<bool> _test) {
			if (!_test()) {
				string message = string.Format("Assertion was not met: {0}",_test.Method.GetMethodBody().ToString());
				Console.Error.WriteLine(message);
				throw new AssertionNotMetException(message);
			}
		}
	}
}
