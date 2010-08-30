
using System;
using System.Drawing;

namespace MagnumHouseLib
{
	public class GangsterTrail : IDrawable, IUpdateable
	{
		private Gangster m_gangster;
		private Sprite m_sprite;
		
		public Layer Layer {get { return Layer.Normal;}}
		public Priority Priority {get { return Priority.Middle; } }
		
		private bool m_dead = false;
		public bool Dead { get { return m_dead; } }
		public void Die() {
			m_dead = true;
		}
		
		public int Id { get { return 0;}}

		public GangsterTrail (Gangster _gangster, Sprite _sprite)
		{
			m_gangster = _gangster;
			m_sprite = _sprite;

//			for (int i = 0 ; i < _sprite.Bitmap.Width ; i++) {
//				for (int j = 0 ; j < _sprite.Bitmap.Height; j++) {
//					m_sprite.Bitmap.SetPixel(i, j, Color.Green);
//				}
//			}
			//m_sprite.ReloadBitmap();
			
			m_sprite.Size.Y = m_sprite.Bitmap.Size.Height / Tile.Size;
			m_sprite.Size.X = m_sprite.Bitmap.Size.Width / Tile.Size;
		}
		
		public void Update(float delta) {
//			Vector2f pos = m_gangster.Position;
//			int off_x = (int)Math.Round(pos.X);
//			int off_y = (int)Math.Round(pos.Y);
//			for (int i = 0 ; i < m_gangster.Size.X ; i++) {
//				for (int j = 0 ; j < m_gangster.Size.Y; j++) {
//					m_sprite.Bitmap.SetPixel(i + off_x, j + off_y, Color.Red);
//				}
//			}
			//m_sprite.ReloadBitmap();
		}
		
		public void Draw() {
			m_sprite.Draw();
		}
	}
}
