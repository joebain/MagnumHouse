
using System;

namespace MagnumHouseLib
{


	public abstract class OkCancelGuiSet : GuiSet
	{

		public OkCancelGuiSet (Game _game, UserInput _keyboard, EditorLevel _editor) :
			base (_game, _keyboard, _editor)
		{
			Button okButton = new Button(m_keyboard, "OK", m_game.Camera);
			okButton.Right();
			okButton.Position = new Vector2f(okButton.Position.X, 1);
			Button cancelButton = new Button(m_keyboard, "Cancel", m_game.Camera);
			cancelButton.Right();
			cancelButton.Position = new Vector2f(cancelButton.Position.X, 3);
			
			okButton.Pressed += Ok;
			cancelButton.Pressed += Cancel;
			
			items.Add(okButton);
			items.Add(cancelButton);
		}
		
		public abstract void Ok();
		public abstract void Cancel();
	}
}
