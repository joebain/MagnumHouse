
using System;
using System.Collections.Generic;

namespace MagnumHouseLib
{
	public abstract class GuiSet : IUpdateable, IDrawable
	{
		public bool Dead { get {return m_dead;}}
		bool m_dead = false;
		public int Id{get{return 0;}}
		
		public Layer Layer {get{return Layer.Normal;}}
		public Priority Priority {get{return Priority.Front;}}
		
		protected SyncList<GuiItem> items = new SyncList<GuiItem>();
		protected SyncList<Text> textItems = new SyncList<Text>();
		
		protected Game m_game;
		protected UserInput m_keyboard;
		protected EditorLevel m_editor;
		
		protected bool buttonClicked = false;
		
		public GuiSet (Game _game, UserInput _keyboard, EditorLevel _editor)
		{
			m_game = _game;
			m_keyboard = _keyboard;
			m_editor = _editor;
		}
		
		public virtual void Update(float _delta) {
			items.NiftyFor<GuiItem>(_i => {
				//if (!(_i is ResizeableBox)) _i.Update(_delta);
				_i.Update(_delta);
			},
			_i => _i.Dead);
			items.Process();
		}
		
		public virtual void Draw() {
			items.NiftyFor<GuiItem>(_i => _i.Draw(), _i => _i.Dead);
			items.Process();
			textItems.NiftyFor<Text>(_i => _i.Draw(), _i => _i.Dead);
			textItems.Process();
		}
		
		public void Die() {
			m_dead = true;
			items.ForEach(_i => _i.Die());
			textItems.ForEach(_i => _i.Die());
		}
		
		protected TextBox NewTextBox(string label, string text, int pos) {
			Text textLabel = new Text(label);
			textLabel.SetHUD(m_game.Camera);
			textLabel.Position = new Vector2f(1, pos*2+3);
			textItems.Add(textLabel);
			
			TextBox textBox = new TextBox(m_keyboard, new Vector2f(10, 2), m_game.Camera);
			textBox.Position = new Vector2f(1, pos*2+1);
			textBox.Pressed += () => {buttonClicked = true;};
			textBox.SetLabel(textLabel);
			textBox.Text = text;
			items.Add(textBox);
			
			return textBox;
		}
		
		protected Button NewButton(string text, int pos, Action action) {
			Button button = new Button(m_keyboard, text, m_game.Camera) {
				Position = new Vector2f(1, pos*2 + 1)
			};
			button.Pressed += () => {
				buttonClicked = true;
				action();
			};
			items.Add(button);
			return button;
		}
		
		protected Text NewLabel(string text, int pos) {
			Text label = new Text(text);
			label.SetHUD(m_game.Camera);
			label.Position = new Vector2f(1, pos*2+1);
			textItems.Add(label);
			return label;
		}
		
		protected ResizeableBox NewBox(Vector2f pos, Vector2f size, Colour colour) {
			ResizeableBox box = new ResizeableBox(m_keyboard, size, null);
			box.Position = pos;
			box.backgroundColour = colour;
			
			items.Add(box);
			return box;
		}
		
		protected ResizeableBox NewBox(BoundingBox bb, Colour colour) {
			ResizeableBox box = new ResizeableBox(m_keyboard, bb.Size, null);
			box.Bounds = bb;
			box.backgroundColour = colour;
			
			items.Add(box);
			return box;
		}
	}
}
