using System;
using System.Collections.Generic;
using System.Linq;

namespace MagnumHouse
{
	public enum Bumped {
		Top, Bottom, Right, Left
	}
	
	public abstract class BumblingThing : Thing2D
	{
		protected Vector2f m_position = new Vector2f();
		protected TileMap m_tiles;
		
		private const float pushAway = 0.0001f;
		
		protected BumblingThing(TileMap _map) {
			m_tiles = _map;
		}
		
		protected IEnumerable<Bumped> TryMove(Vector2f _move) {
			List<Bumped> bumps = new List<Bumped>();
			m_position.Y += _move.Y;
			if (m_tiles.IsCollision(m_position, Size)) {
				if (_move.Y > 0) {
					m_position.Y = ((int)m_position.Y) + (1f - Size.Y) - pushAway;
					bumps.Add(Bumped.Top); 
				} else if (_move.Y < 0) {
					m_position.Y = ((int)(m_position.Y + 1)) + pushAway;
					bumps.Add(Bumped.Bottom);
				}
			}
			
			m_position.X += _move.X;
			if (m_tiles.IsCollision(m_position, Size)) {
				if (_move.X > 0) {
					m_position.X = ((int)m_position.X) + (1f - Size.X) - pushAway;
					bumps.Add(Bumped.Right);
				} else if (_move.X < 0) {
					m_position.X = ((int)(m_position.X + 1)) + pushAway;
					bumps.Add(Bumped.Left);
				}
			}
			
			return bumps;
		}
		
		protected void PrintBumps(IEnumerable<Bumped> _bumps) {
			if (_bumps.Contains(Bumped.Left))
			    Console.Write(">");
			if (_bumps.Contains(Bumped.Right))
				Console.Write("<");
			if (_bumps.Contains(Bumped.Top))
				Console.Write(@"\/");
			if (_bumps.Contains(Bumped.Bottom))
				Console.Write(@"/\");
			Console.WriteLine();
		}
	}
}
