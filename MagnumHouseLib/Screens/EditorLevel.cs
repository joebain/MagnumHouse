
using System;
using Tao.Sdl;
using System.Collections.Generic;

namespace MagnumHouseLib
{
	public class EditorLevel : Screen
	{
		float cameraSpeed = 10f;
		TileMap m_map;
		public TileMap Map {get { return m_map;}}
		EditorCursor cursor;
		Thing2D cameraSubject = new Thing2D();
		
		GuiSet m_guiset;
		public EditorCursor Cursor {get{return cursor;}}
		BoundingBox camSubBounds;
		
		public string filename = "default";
		
		public EditorLevel ()
		{
			m_size = new Vector2i(100,100);
			m_bounds.Top = 100;
			m_bounds.Right = 100;
			m_bounds.Left = -20;
		}
		
		
		public void ChangeGuiSet(GuiSet guiSet) {
			m_guiset.Die();
			m_guiset = guiSet;
			m_house.AddDrawable(m_guiset);
			m_house.AddUpdateable(m_guiset);
		}
		
		public override void Setup (Game _game, UserInput _keyboard, ScreenMessage _message)
		{
			base.Setup (_game, _keyboard, _message);
			
			m_game.Camera.CameraSubject = cameraSubject;
			camSubBounds = Bounds.Clone;
			camSubBounds.Left += Game.Size.X/2;
			camSubBounds.Bottom += Game.Size.Y/2;
			
			m_game.Camera.ViewOffset = _message.Position;
			m_game.AddLevel(new EditorPlayLevel());
			
			if (_message.Level != null) {
				m_map = _message.Level;
			} else {
				m_map = new TileMap(Size);
			}
			m_house.AddDrawable(m_map);
			
			GraphBackground bg = new GraphBackground(Size);
			bg.Layer = Layer.Normal;
			m_house.AddDrawable(bg);
			
			cursor = new EditorCursor(m_keyboard, Bounds.Clone);
			cursor.Position = m_keyboard.MousePos;
			m_house.AddDrawable(cursor);
			m_house.AddUpdateable(cursor);
			
			m_guiset = new StandardGuiSet(m_game, m_keyboard, this);
			m_house.AddDrawable(m_guiset);
			m_house.AddUpdateable(m_guiset);
			
			m_house.ProcessLists();
		}
		
		public override void Update (float _delta)
		{
			
			//keys move the camera
			if (m_keyboard.IsKeyPressed(Sdl.SDLK_UP)) {
				cameraSubject.Position.Y += cameraSpeed*_delta;
			} else if (m_keyboard.IsKeyPressed(Sdl.SDLK_DOWN)) {
				cameraSubject.Position.Y -= cameraSpeed*_delta;
			}
			if (m_keyboard.IsKeyPressed(Sdl.SDLK_LEFT)) {
				cameraSubject.Position.X -= cameraSpeed*_delta;
			} else if (m_keyboard.IsKeyPressed(Sdl.SDLK_RIGHT)) {
				cameraSubject.Position.X += cameraSpeed*_delta;
			}
			
			//mouse moves the camera at edge
			if (Math.Abs(m_keyboard.MousePos.X - cameraSubject.Position.X) > Game.Width/2 - 0.2f ||
			    Math.Abs(m_keyboard.MousePos.Y - cameraSubject.Position.Y) > Game.Height/2 - 0.2f) {
				Vector2f move = (m_keyboard.MousePos - cameraSubject.Position);
				move.Cap(cameraSpeed*_delta);
				cameraSubject.Position += move;
			}
			
			cameraSubject.Position.Clamp(camSubBounds);
		}
	}
}

