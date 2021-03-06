using Tao.Sdl;
using System;

namespace MagnumHouseLib
{
	public class StandardGuiSet : GuiSet
	{
		Button spikyButton;
		Button blockButton;
		Button floorButton;
		Button emptyButton;
		
		Button playButton;
		Button locationsButton;
		Button fileButton;
		
		int m_selectedTileType = TileMap.EMPTY;
		public int SelectedTileType {
			get { return m_selectedTileType;}
			set {
				m_selectedTileType = value;
				m_editor.Cursor.SelectedTileType = value;
			}
		}
		
		int[,] tileClicks;
		
		public StandardGuiSet (Game _game, UserInput _keyboard, EditorLevel _editor) :
			base (_game, _keyboard, _editor)
		{
			tileClicks = new int[m_editor.Size.X, m_editor.Size.Y];
			
			spikyButton = NewChangeTileTypeButton("Spiky", 0, TileMap.SPIKY);
			blockButton = NewChangeTileTypeButton("Block", 1, TileMap.BLOCK);
			floorButton = NewChangeTileTypeButton("Floor", 2, TileMap.FLOOR);
			emptyButton = NewChangeTileTypeButton("Empty", 3, TileMap.EMPTY);
			
			playButton = NewButton("Play", 10, PlayLevel);
			
			fileButton = NewButton("File", 11, FileMode);
			locationsButton = NewButton("Locations", 12, LocationsMode);
		}
		
		public override void Update (float _delta)
		{
			base.Update(_delta);
			
			//check we didnt click on a button
			if (!buttonClicked) {
				//mouse clicks on tiles
				Vector2i mouse_pos = m_editor.Cursor.GridPosition;
				if (mouse_pos.X >= 0 && mouse_pos.X < m_editor.Size.X &&
				    mouse_pos.Y >= 0 && mouse_pos.Y < m_editor.Size.Y) {
					
					if (m_keyboard.IsMouseButtonPressed(Sdl.SDL_BUTTON_LEFT)) {
						
						if (tileClicks[mouse_pos.X, mouse_pos.Y] == 0) {
							tileClicks[mouse_pos.X, mouse_pos.Y] = 1;
							m_editor.Map.SetTileAt(mouse_pos, SelectedTileType);
						}
					} else {
						tileClicks[mouse_pos.X, mouse_pos.Y] = 0;
					}
				}
			}
			else {
				buttonClicked = false;
			}
		}
		
		private Button NewChangeTileTypeButton(string text, int pos, int tiletype) {
			return NewButton(text, pos, () => SelectedTileType = tiletype);
		}
		
		private void LocationsMode() {
			m_editor.ChangeGuiSet(new EditLocationsGuiSet(m_game, m_keyboard, m_editor));
		}
		
		private void FileMode() {
			m_editor.ChangeGuiSet(new FileGuiSet(m_game, m_keyboard, m_editor));
		}
		
		private void PlayLevel() {
			m_editor.Exit(new ScreenMessage() {
				Position = m_game.Camera.ViewOffset + Game.ScreenCentre,
				Level = m_editor.Map
			});
		}
	}
}
