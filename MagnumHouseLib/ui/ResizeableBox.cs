
using System;
using Tao.Sdl;
using Tao.OpenGl;

namespace MagnumHouseLib
{
	public class ResizeableBox : GuiItem
	{
		bool leftGrab;
		bool rightGrab;
		bool topGrab;
		bool bottomGrab;
		bool middleGrab;
		
		BoundingBox right = new BoundingBox();
		BoundingBox left = new BoundingBox();
		BoundingBox top = new BoundingBox();
		BoundingBox bottom = new BoundingBox();
		BoundingBox middle = new BoundingBox();
		
		Vector2f grabPos = new Vector2f();
		
		public Action<BoundingBox> MoveAction;
		public Action<BoundingBox> ResizeAction;
		
		const float bounds = 0.5f;
		
		public ResizeableBox (UserInput keyboard, Vector2f size, Camera camera) : base(keyboard, size, camera)
		{
			keyboard.MouseDown += MouseDownHandler;
			keyboard.MouseUp += MouseUpHandler;
		}
		
		private bool MouseOnBottom() {
			bottom = new BoundingBox(Position.X, Position.Y, Position.X + Size.X, Position.Y+bounds);
			return bottom.Contains(m_keyboard.MousePos);
		}
		
		private bool MouseOnTop() {
			top = new BoundingBox(Position.X, Position.Y+Size.Y-bounds, Position.X + Size.X, Position.Y+Size.Y);
			return top.Contains(m_keyboard.MousePos);
		}
		
		private bool MouseOnLeft ()
		{
			left = new BoundingBox(Position.X, Position.Y, Position.X + bounds, Position.Y + Size.Y);
			return left.Contains(m_keyboard.MousePos);
		}
		
		private bool MouseOnRight ()
		{
			right = new BoundingBox(Position.X + Size.X - bounds, Position.Y, Position.X + Size.X, Position.Y + Size.Y);
			return right.Contains(m_keyboard.MousePos);
		}
		
		private bool MouseInMiddle() {
			middle = new BoundingBox(Position.X + bounds, Position.Y+bounds, Position.X+Size.X-bounds, Position.Y+Size.Y-bounds);
			return middle.Contains(m_keyboard.MousePos);
		}
		
		public void MouseDownHandler(Sdl.SDL_MouseButtonEvent button) {
			if (button.button == Sdl.SDL_BUTTON_LEFT) {
				grabPos = m_keyboard.MousePos - Position;
				if (MouseOnLeft ()) {
					leftGrab = true;
				}
				if (MouseOnRight ()) {
					rightGrab = true;
				}
				if (MouseOnBottom()) {
					bottomGrab = true;
				}
				if (MouseOnTop()) {
					topGrab = true;
				}
				if (MouseInMiddle()) {
					middleGrab = true;
				}
			}
		}
		
		public void MouseUpHandler(Sdl.SDL_MouseButtonEvent button) {
			if (button.button == Sdl.SDL_BUTTON_LEFT) {
				leftGrab = false;
				rightGrab = false;
				topGrab = false;
				bottomGrab = false;
				middleGrab = false;
			}
		}
		
		public override void Update (float _delta)
		{
			if (middleGrab) {
				Position = m_keyboard.MousePos - grabPos;
				if (MoveAction != null) MoveAction(Bounds);
			}
			if (rightGrab) {
				Size.X = m_keyboard.MousePos.X - Position.X;
				if (ResizeAction != null) ResizeAction(Bounds);
			}
			if (leftGrab) {
				float oldx = Position.X;
				Position = new Vector2f(m_keyboard.MousePos.X, Position.Y);
				Size.X += oldx - Position.X;
				if (ResizeAction != null) ResizeAction(Bounds);
			}
			if (bottomGrab) {
				float oldy = Position.Y;
				Position = new Vector2f(Position.X, m_keyboard.MousePos.Y);
				Size.Y += oldy - Position.Y;
				if (ResizeAction != null) ResizeAction(Bounds);
			}
			if (topGrab) {
				Size.Y = m_keyboard.MousePos.Y - Position.Y;
				if (ResizeAction != null) ResizeAction(Bounds);
			}
			base.Update (_delta);
		}
		
		public override void Draw ()
		{
			base.Draw ();
			if (MouseOnBottom()) {
				bottom.Draw(foregroundColour);
			}
			if (MouseOnTop()) {
				top.Draw(foregroundColour);
			}
			if (MouseOnRight()) {
				right.Draw(foregroundColour);
			}
			if (MouseOnLeft()) {
				left.Draw(foregroundColour);
			}
		}
		
		public override void Die ()
		{
			base.Die ();
			m_keyboard.MouseDown -= MouseDownHandler;
			m_keyboard.MouseUp -= MouseUpHandler;
		}
	}
}
