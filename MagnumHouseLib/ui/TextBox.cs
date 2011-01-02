
using System;
using Tao.Sdl;

namespace MagnumHouseLib
{
	public class TextBox : GuiItem
	{
		private Text Contents;
		private Text m_label;
		public string Text { get { return Contents.Contents;} set{Contents.Contents = value;}}
		public override bool Hidden {
			get {
				return base.Hidden;
			}
			set {
				base.Hidden = value;
				if (m_label != null) m_label.Hidden = value;
			}
		}

		
		public override Vector2f Position {
			get {
				return base.Position;
			}
			set {
				base.Position = value.Clone();
				Contents.Position = value.Clone();
			}
		}
		
		public TextBox (UserInput input, Vector2f size, Camera camera)
			: base (input, size, camera)
		{
			Contents = new Text("");
			if (camera != null)
				Contents.SetHUD(camera);
			
			m_keyboard.KeyDown += AddLetter;
		}
		
		public void AddLetter(Sdl.SDL_keysym key) {
			if (!HasFocus) return;
			if (m_keyboard.KeyIsChar(key)) {
				Contents.Contents += m_keyboard.KeyToChar(key);
			} else if (key.sym == Sdl.SDLK_BACKSPACE && Contents.Contents.Length > 0) {
				Contents.Contents = Contents.Contents.Substring(0, Contents.Contents.Length-1);
			}
		}
		
		public void SetLabel(Text label) {
			m_label = label;
		}
		
		public override void Draw ()
		{
			if (Hidden) return;
			base.Draw ();
			Contents.Draw();
		}
		
		public void Die ()
		{
			base.Die();
			if (m_label != null) m_label.Die();
			m_keyboard.KeyDown -= AddLetter;
		}
	}
}
