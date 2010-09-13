
using System;

namespace MagnumHouseLib
{
	[Serializable]
	public abstract class TriggerDescription {
		public abstract Trigger MakeReal(Screen level);
	}
	
	[Serializable]
	public class SpatialTriggerDescription : TriggerDescription {
		public BoundingBox boxOn, boxOff;
		
		public override Trigger MakeReal(Screen level) {
			return new SpatialTrigger(boxOn, boxOff, level.Character) {};
		}
	}
	
	[Serializable]
	public class TimedTriggerDescription : TriggerDescription {
		public float start, length;
		
		public override Trigger MakeReal(Screen level) {
			return new TimedTrigger(start, length);
		}
	}


	[Serializable]
	public abstract class EventDescription
	{
		public TriggerDescription trigger;
		
		public abstract Event MakeReal(Screen level);
	}
	
	[Serializable]
	public class TextEventDescription : EventDescription {
		public string text;
		public Vector2f pos;
		public bool hud;
		
		public override Event MakeReal(Screen level) {
			Text gText = new Text(text);
			gText.Position = pos;
			if (hud) gText.SetHUD(level.Game.Camera);
			return new TextEvent(trigger.MakeReal(level), gText, level.House);
		}
	}
}
