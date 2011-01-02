using System;

namespace MagnumHouseLib
{
	public class Button : GuiItem
	{
		Text Contents;
		
		public override Vector2f Position {
			get {
				return base.Position;
			}
			set {
				base.Position = value;
				Contents.Position = value;
			}
		}
		
		public Button (UserInput cursor, string text, Camera camera) : base(cursor, new Vector2f(1,1), camera)
		{
			Contents = new Text(text);
			Contents.SetHUD(camera);
			Size = Contents.Size;
		}
		
		public override void Draw() {
			if (Hidden) return;
			Contents.Colour = foregroundColour;
			
			base.Draw();
			Contents.Draw();
		}
	}
}
