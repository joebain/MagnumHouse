
using System;
using System.Drawing;

namespace MagnumHouseLib
{
	public class Text : Thing2D, IDrawable, IUpdateable
	{
		public Layer Layer { get;set;}
		
		string m_text;
		Sprite m_sprite = new Sprite();
		static Bitmap tmpBmp = new Bitmap(1024, 1024);
		Font font = new Font(FontFamily.GenericMonospace, 22);
		
		public Text ()
		{
			ChangeText("");
			Layer = Layer.Normal;
		}
		
		public Text(string _text) {
			ChangeText(_text);
			Layer = Layer.Normal;
		}
		
		public void ChangeTextQuickly(string _text) {
			m_text = _text;
			
			var g = Graphics.FromImage(m_sprite.Bitmap);
			g.DrawString(m_text, font, Brushes.White, new PointF(0,0));
			m_sprite.ReloadBitmap();
		}
		
		public void ChangeText(string _text) {
			m_text = _text;
			
			Graphics g = Graphics.FromImage(tmpBmp);
			
			var size = g.MeasureString(m_text, font);
			
			Bitmap bitmap = new Bitmap((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height));
			g = Graphics.FromImage(bitmap);
			g.DrawString(m_text, font, Brushes.White, new PointF(0,0));
			
			m_sprite.Bitmap = bitmap;
			m_sprite.ReloadBitmap();
			//m_sprite = new Sprite(bitmap);
			
			Size = new Vector2f(size.Width / Tile.Size, size.Height / Tile.Size);
		}
		
		public override Vector2f Size {
			get {
				return m_sprite.Size;
			}
			set {
				m_sprite.Size = value;
			}
		}

		public override Vector2f Position {
			get {
				return m_sprite.Position;
			}
			set {
				m_sprite.Position = value;
			}
		}
		
		public override void SetHUD (Camera _camera)
		{
			m_sprite.SetHUD (_camera);
		}
		
		public void Draw() {
			m_sprite.Draw();
		}
		
		public Action<float> updateAction;
		
		public void Update(float _delta) {
			if (updateAction != null) updateAction(_delta);
		}
	}
}
