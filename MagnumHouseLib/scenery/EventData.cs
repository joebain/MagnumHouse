
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MagnumHouseLib
{
	[Serializable]
	public class EventData
	{
		public List<EventDescription> Events = new List<EventDescription>();
		
//		public override string ToString ()
//		{
//			string s = "";
//			
//			foreach (EventDescription e in Events) {
//				switch (e.GetType()) {
//				case TextEventDescription:
//					s = string.Concat(s, GetString(e), "\n");
//					break;
//				}
//			}
//			return s;
//		}
//		
//		public void FromString(String s) {
//			foreach (string line in s.Split("\n")) {
//				string[] parts = line.Split("@!");
//				if (parts[0] == "TYPE:") {
//					switch (parts[0]) {
//					case typeof(TextEventDescription).Name:
//						TextEventDescription desc = GetTED(line);
//						Events.Add(desc);
//						break;
//					}
//				} else {
//					throw new TypeLoadException("WAAHAHAHAH!!!!");	
//				}
//			}
//		}
//		
//		private string GetString(TextEventDescription e) {
//			string s = "";
//			string.Concat("TYPE:", typeof(TextEventDescription).Name,
//			              "@!posx:", e.pos.X,
//			              "@!posy:", e.pos.Y,
//			              "@!hud:" , e.hud,
//			              "@!text:", e.text,
//			              "@!trigger:", "cunt");
//		}
	}
}
