
using System;

namespace MagnumHouseLib
{
	public class FileGuiSet : GuiSet
	{
		Button saveButton;
		Button loadButton;
		Button backButton;
		
		TextBox filenameBox;
		
		NumberBox widthBox;
		Button widthPlusButton;
		Button widthMinusButton;
		NumberBox heightBox;
		Button heightPlusButton;
		Button heightMinusButton;
		
		public FileGuiSet (Game _game, UserInput _keyboard, EditorLevel _editor) :
			base (_game, _keyboard, _editor)
		{
			saveButton = NewButton("Save", 3, SaveLevel);
			loadButton = NewButton("Load", 4, LoadLevel);
			filenameBox = NewTextBox("Filename", m_editor.filename, 5);
			
			widthBox = new NumberBox(m_keyboard, new Vector2f(5,2), m_game.Camera);
			widthBox.Position = new Vector2f(1, 4);
			items.Add(widthBox);
			widthMinusButton = new Button(m_keyboard, "-", m_game.Camera);
			widthMinusButton.Position = new Vector2f(1 + widthBox.Size.X + 0.5f, 4);
			items.Add(widthMinusButton);
			widthPlusButton = new Button(m_keyboard, "+", m_game.Camera);
			widthPlusButton.Position = new Vector2f(6.5f + widthMinusButton.Size.X + 0.5f, 4);
			items.Add(widthPlusButton);
			
			backButton = NewButton("Back", 0, GoBack);
		}
		
		private void SaveLevel() {
			m_editor.filename = filenameBox.Text;
			m_editor.Map.Save(filenameBox.Text);
		}
		
		private void LoadLevel() {
			m_editor.filename = filenameBox.Text;
			m_editor.Map.Load(filenameBox.Text);
		}
		
		private void GoBack() {
			m_editor.ChangeGuiSet(new StandardGuiSet(m_game, m_keyboard, m_editor));
		}
	}
}
