
using System;
using System.Reflection;
using System.Collections.Generic;

namespace MagnumHouseLib
{
	public class GeneralGuiSet<T> : OkCancelGuiSet where T : new()
	{
		Action<T> m_ok;
		Action m_cancel;
		
		Dictionary<FieldInfo, Func<object>> fieldValues = new Dictionary<FieldInfo, Func<object>>();
		
		public GeneralGuiSet(Game _game, UserInput _keyboard, EditorLevel _editor, Action<T> ok, Action cancel) :
			base (_game, _keyboard, _editor)
		{
			m_ok = ok;
			m_cancel = cancel;
			
			Type type = typeof(T);
			FieldInfo[] fields = type.GetFields();
			Console.WriteLine("t is {0} there are {1} fields", type.FullName, fields.Length);
			Vector2f position = new Vector2f(1,1);
			foreach (FieldInfo field in fields) {
				GuiItem item = MakeGuiItem(field, position);
			}
		}
		
		private GuiItem MakeGuiItem(FieldInfo field, Vector2f position) {
			GuiItem item = null;
			Type t = field.FieldType;
			if (t == typeof(string)) {
				Console.WriteLine("adding string");
				var textBox = new TextBox(m_keyboard, new Vector2f(10,2), m_game.Camera);
				textBox.backgroundColour = new Colour(1,0.2f,0.8f,1);
				fieldValues[field] = () => textBox.Text;
				item = textBox;
			} else if (t == typeof(stringAndPos)) {
				Console.WriteLine("adding string and pos");
				var textBox = new MoveableTextBox(m_keyboard, new Vector2f(10,2), m_game.Camera);
				textBox.backgroundColour = new Colour(1,0.2f,0.8f,1);
				fieldValues[field] = () => new stringAndPos() {text = textBox.Text, pos = textBox.Position};
				item = textBox;
			} else if (t == typeof(BoundingBox)) {
				Console.WriteLine("adding bb");
				var box = new ResizeableBox(m_keyboard, new Vector2f(5,5), m_game.Camera);
				box.backgroundColour = new Colour(1,0.2f,0.8f,1);
				fieldValues[field] = () => box.Bounds;
				item = box;
			} else if (t == typeof(float)) {
				Console.WriteLine("adding number");
				var numberBox = new NumberBox(m_keyboard, new Vector2f(10,2), m_game.Camera);
				numberBox.backgroundColour = new Colour(1,0.2f,0.8f,1);
				fieldValues[field] = () => (float)numberBox.Number;
				item = numberBox;
			} else if (t == typeof(Colour)) {
				Console.WriteLine("adding colour");
				var colourBox = new ColourTextBox(m_keyboard, new Vector2f(10,2), m_game.Camera);
				colourBox.backgroundColour = new Colour(1,0.2f,0.8f,1);
				fieldValues[field] = () => colourBox.Colour;
				colourBox.Text = "0,0,0,0";
				item = colourBox;
			} else if (t == typeof(bool)) {
				Console.WriteLine("adding bool");
				var tickBox = new TickBox(m_keyboard, m_game.Camera);
				fieldValues[field] = () => tickBox.Ticked;
				item = tickBox;
			}
			if (item == null) return null;
			item.Position = position.Clone();
			position.Y += item.Size.Y + 1;
			item.SetLabel(field.Name);
			items.Add(item);
			return item;
		}
		
		public override void Ok ()
		{
			T result = new T();
			foreach (var field in result.GetType().GetFields()) {
				Func<object> getval;
				if (fieldValues.TryGetValue(field, out getval)) {
					field.SetValue(result, getval());
					Console.WriteLine("setting a value of {0} for {1}", getval(), field.Name);
				}
			}
			m_ok(result);
		}
		
		public override void Cancel ()
		{
			m_cancel();
		}
	}
}
