
using System;
namespace MagnumHouseLib
{
	[Serializable]
	public class TextEventDescription : EventDescription {
		public stringAndPos textAndPos;
		public bool hud;
		
		public override Event MakeReal(Screen level) {
			Text gText = new Text(textAndPos.text);
			gText.Position = textAndPos.pos;
			if (hud) gText.SetHUD(level.Game.Camera);
			return new TextEvent(trigger.MakeReal(level), gText, level.House);
		}
	}
}
