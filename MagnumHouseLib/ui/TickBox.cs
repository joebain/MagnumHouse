
using System;
using Tao.OpenGl;

namespace MagnumHouseLib
{
	public class TickBox : GuiItem
	{
		private const float tickSpacing = 0.2f;
		float m_tickCounter = 0;
		
		public TickBox (UserInput input, Camera camera)
			: base (input, new Vector2f(1,1), camera)
		{
			Pressed += ToggleTicked;
		}
		
		public bool Ticked;
		
		protected override void DrawAfter ()
		{
			if (Ticked) {
				Gl.glBegin(Gl.GL_LINES);
				Gl.glColor3f(1,1,1);
				Gl.glVertex2f(-Size.X/2, -Size.Y/2);
				Gl.glVertex2f(Size.X/2, Size.Y/2);
				Gl.glVertex2f(-Size.X/2, Size.Y/2);
				Gl.glVertex2f(Size.X/2, -Size.Y/2);
				Gl.glEnd();
			}
		}
		
		public override void Update (float _delta)
		{
			base.Update (_delta);
			if (m_tickCounter > 0) {
				m_tickCounter -= _delta;
			}
		}
		
		public void ToggleTicked() {
			if (m_tickCounter <= 0) {
				m_tickCounter = tickSpacing;
				Ticked = !Ticked;
			}
		}
	}
}
