
using System;
using Tao.OpenGl;
using Tao.Sdl;

namespace MagnumHouseLib
{
	public class EditorCursor : Thing2D, IDrawable, IUpdateable
	{
		UserInput m_keys;
		
		bool m_dead = false;
		public int SelectedTileType = TileMap.EMPTY;
		
		public bool IsClicked { get { return m_keys.IsMouseButtonPressed(Sdl.SDL_BUTTON_LEFT); }}
		
		public override Vector2f Position {
			get {
				return base.Position;
			}
			set {
				base.Position = value;
				Position.Clamp(Vector2f.Zero, m_bounds.ToF());
			}
		}

		public Vector2i GridPosition { get { return (Position-new Vector2f(0.5f)).Round(); }}
		
		public Layer Layer {get { return Layer.Normal; }}
		public Priority Priority {get { return Priority.Middle;}}
		public int Id {get{ return 0;}}
		
		Vector2i m_bounds;
		
		public EditorCursor (UserInput keys, Vector2i bounds)
		{
			m_keys = keys;
			m_bounds = bounds;
		}
		
		public void Update(float _delta) {
			Position = m_keys.MousePos;
		}
		
		public void Draw() {
			Gl.glPushMatrix();
			Gl.glTranslatef(Position.X, Position.Y, 0);
			
			Gl.glBegin(Gl.GL_LINES);
			Gl.glColor3f(1,1,1);
			
			Gl.glVertex2f(1,-0.5f);
			Gl.glVertex2f(0,0);
			Gl.glVertex2f(0,0);
			Gl.glVertex2f(0.5f,-1);
			
			Gl.glEnd();
			
			Gl.glPopMatrix();
			
			switch (SelectedTileType) {
			case TileMap.BLOCK:
				Tile.Draw(GridPosition);
				break;
			case TileMap.FLOOR:
				FloorTile.Draw(GridPosition);
				break;
			case TileMap.SPIKY:
				SpikyTile.Draw(GridPosition);
				break;
			default:
				break;
			}
		}
		
		public bool Dead {get { return m_dead;}}
		public void Die() { m_dead = true;}
	}
}
