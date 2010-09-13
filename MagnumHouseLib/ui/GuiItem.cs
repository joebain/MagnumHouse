using Tao.OpenGl;
using Tao.Sdl;

using System;

namespace MagnumHouseLib
{
	public abstract class GuiItem : Thing2D, IDrawable, IUpdateable
	{
		protected UserInput m_keyboard;
		public Colour backgroundColour;
		public Colour foregroundColour;
		
		public Layer Layer {get;set;}
		public Priority Priority {get;set;}
		private bool m_dead = false;
		public bool Dead { get {return m_dead;}}
		public int Id = 0;
		int displayList;
		public virtual bool Hidden {get; set;}
		
		float timeSinceTouched;
		
		//this doesnt need to be a vector but it has to be
		//*something* for locking to work
		static object s_hasFocus = new Vector2f();
		
		static bool GetFocus(object _taker) {
			if (_taker == null) return false;
			lock (s_hasFocus) {
				s_hasFocus = _taker;
				return true;
			}
			return false;
		}
		
		public bool HasFocus { get { return s_hasFocus == this;}}
		
//		static bool ReturnFocus(object _giver) {
//			if (_giver != s_hasFocus || s_focus == false) return false;
//			lock (s_focus) {
//				s_focus = false;
//				s_hasFocus = null;
//			}
//			return true;
//		}

		public Action Pressed;
		
		public GuiItem (UserInput keyboard, Vector2f size, Camera camera)
		{
			m_keyboard = keyboard;
			m_keyboard.MouseDown += MouseClicked;
			
			Size = size;
			backgroundColour = new Colour(1,0.4f,0.4f,0.5f);
			foregroundColour = new Colour(1,1,1,1);
			Hidden = false;
			
			if (camera != null)
				SetHUD(camera);
			
			Layer = Layer.Normal;
			Priority = Priority.Front;
			
			displayList = Gl.glGenLists(1);
			Gl.glNewList(1, Gl.GL_COMPILE);
			ConstantDraw();
			Gl.glEndList();
		}
		
		public virtual void Die() {
			m_dead = true;
			m_keyboard.MouseDown -= MouseClicked;
		}
		
		public virtual void MouseClicked(Sdl.SDL_MouseButtonEvent button) {
			if (Hidden) return;
			if (button.button == Sdl.SDL_BUTTON_LEFT && Bounds.Contains(m_keyboard.MousePos)) {
				if (Pressed != null) Pressed();
				GetFocus(this);
			}
		}
		
		public virtual void Update(float _delta) {
			if (Hidden) return;
			
			if (Bounds.Contains(m_keyboard.MousePos)) {
				backgroundColour.A = 1f;
			} else {
				backgroundColour.A = 0.5f;
			}
		}
		
		public virtual void Draw() {
			if (Hidden) return;
			
			Gl.glPushMatrix();
			
			Gl.glTranslatef(Position.X, Position.Y, 0);
			
			Gl.glColor4f(backgroundColour.R, backgroundColour.G, backgroundColour.B, backgroundColour.A);
			
			Gl.glEnable(Gl.GL_BLEND);
			Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
			
			//Gl.glCallList(displayList);
			DrawBefore();
			ConstantDraw();
			DrawAfter();
			
			Gl.glDisable(Gl.GL_BLEND);
			
			Gl.glPopMatrix();
		}
		
		protected virtual void DrawAfter() {
			
		}
		
		protected virtual void DrawBefore() {
			if (HasFocus) {
				Gl.glColor3f(1,1,1);
				Gl.glBegin(Gl.GL_LINE_LOOP);
				
				Gl.glVertex2f(-0.1f,-0.1f);
				Gl.glVertex2f(-0.1f,Size.Y+0.1f);
				Gl.glVertex2f(Size.X+0.1f,Size.Y+0.1f);
				Gl.glVertex2f(Size.X+0.1f,-0.1f);
				
				Gl.glEnd();
				
				Gl.glColor4fv(backgroundColour.Array);
			}
		}
		
		protected virtual void ConstantDraw() {
			Gl.glBegin(Gl.GL_TRIANGLE_FAN);
			
			Gl.glVertex2f(0,0);
			Gl.glVertex2f(0,Size.Y);
			Gl.glVertex2f(Size.X,Size.Y);
			Gl.glVertex2f(Size.X,0);
			
			Gl.glEnd();
		}
	}
}
