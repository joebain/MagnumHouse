using System;
using Tao.Sdl;
using Tao.OpenGl;
namespace MagnumHouseLib
{
	public class MoveablePoint : GuiItem
	{
		private Boolean grab;
		private Vector2f grabPos;
		
		public Action<Vector2f> MoveAction;
		
		public MoveablePoint (UserInput keyboard, Camera camera) :
			this(keyboard, new Vector2f(2), camera)
		{
		}
		
		private MoveablePoint (UserInput keyboard, Vector2f size, Camera camera) : base(keyboard, size, camera)
		{
			keyboard.MouseDown += MouseDownHandler;
			keyboard.MouseUp += MouseUpHandler;
		}
		
		public void MouseDownHandler(Sdl.SDL_MouseButtonEvent button) {
			if (button.button == Sdl.SDL_BUTTON_LEFT) {
				grabPos = m_keyboard.MousePos - Position;
				
				grab = Bounds.Contains(m_keyboard.MousePos);
			}
		}
		
		public void MouseUpHandler(Sdl.SDL_MouseButtonEvent button) {
			if (button.button == Sdl.SDL_BUTTON_LEFT) {
				grab = false;
			}
		}
		
		public override void Update (float _delta)
		{
			if (grab) {
				Position = m_keyboard.MousePos - grabPos;
				if (MoveAction != null) MoveAction(Bounds.Centre);
			}
			base.Update(_delta);
		}
		
		protected override void DrawAfter ()
		{
			Gl.glBegin(Gl.GL_LINES);
			Gl.glColor3f(1,1,1);
			Gl.glVertex2f(Size.X/2, 0);
			Gl.glVertex2f(Size.X/2, Size.Y);
			Gl.glVertex2f(0, Size.Y/2);
			Gl.glVertex2f(Size.X, Size.Y/2);
			Gl.glEnd();
		}
		
		public override void Die ()
		{
			base.Die ();
			m_keyboard.MouseDown -= MouseDownHandler;
			m_keyboard.MouseUp -= MouseUpHandler;
		}
	}
}

