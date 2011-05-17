
using System;
using System.Collections.Generic;
using Tao.Sdl;

namespace MagnumHouseLib
{
	public class EditLocationsGuiSet : GuiSet
	{
		Button setStartLocationButton;
		Button setEndLocationButton;
		Button addPointButton;
		Button addBoxButton;
		Button deleteButton;
		Button exitButton;
		
		Text label;
		
		int locationCount = 0;
		
		public EditLocationsGuiSet (Game _game, UserInput _keyboard, EditorLevel _editor) :
			base (_game, _keyboard, _editor)
		{
			exitButton = NewButton("Back", 0, Exit);
			
			label = NewLabel("Add:", 6);
			
			addBoxButton = NewButton("Box", 4, () => AddBox("box"+(locationCount++)));
			addPointButton = NewButton("Point", 5, () => AddPoint("point"+(locationCount++)));
			
			deleteButton = NewButton("Delete", 2, Delete);
			
			foreach (BoxDescription box in m_editor.Map.locationData.boxes) {
				makeBox(box.name, box.box);
			}
			foreach (PointDescription point in m_editor.Map.locationData.points) {
				makePoint(point.name, point.point);
			}
		}
		
		private ResizeableBox makeBox(String title) {
			Vector2f bottomLeft = m_game.Camera.CameraSubject.Position;
			BoundingBox box = new BoundingBox(bottomLeft, bottomLeft + new Vector2f(5));
			return makeBox(title, box);
		}
				        
		private ResizeableBox makeBox(String title, BoundingBox box) {
			var rBox = new ResizeableBox(m_keyboard, new Vector2f(), null);
			rBox.backgroundColour = new Colour(1,0.2f,0.8f,1);
			rBox.Bounds = box.Clone;
			rBox.SetLabel(title);
			items.Add(rBox);
			return rBox;
		}
		
		private void AddBox(String title) {
			var box = makeBox(title);
			var boxDesc = new BoxDescription(title, box.Box);
			m_editor.Map.locationData.boxes.Add(boxDesc);
			box.MoveAction = _box => boxDesc.box = _box;
			box.ResizeAction = _box => boxDesc.box = _box;
		}
		
		private MoveablePoint makePoint(String title) {
			return makePoint(title, m_game.Camera.CameraSubject.Position);
		}
		
		private MoveablePoint makePoint(String title, Vector2f point) {
			var mPoint = new MoveablePoint(m_keyboard, null);
			mPoint.backgroundColour = new Colour(1,0.2f,0.8f,1);
			mPoint.CentreOn(point);
			mPoint.SetLabel(title);
			items.Add(mPoint);
			return mPoint;
		}
		
		private void AddPoint(String title) {
			var point = makePoint(title);
			var pointDesc = new PointDescription(title, point.Bounds.Centre);
			m_editor.Map.locationData.points.Add(pointDesc);
			point.MoveAction = _point => pointDesc.point = _point;
		}
		
		private void Exit() {
			m_editor.ChangeGuiSet(new StandardGuiSet(m_game, m_keyboard, m_editor));
		}
		
		private void Delete() {
			
			object focused = GuiItem.Focused;
			if (focused is ResizeableBox) {
				ResizeableBox eventBox = (ResizeableBox) focused;
				
			}
		}
	}
}
