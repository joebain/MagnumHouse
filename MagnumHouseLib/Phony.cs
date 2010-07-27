using System;
using System.Linq;
using System.Collections.Generic;
using Tao.OpenGl;

namespace MagnumHouseLib
{
	public class Phony : Gangster
	{

		Vector2f m_direction = Vector2f.Left;
		
		Vector2f m_aim = new Vector2f();
		
		private const float distToEnemySquared = 40f;
		
		public override float maxFloorSpeed {
			get {
				return MaxFloorSpeed/5;
			}
		}



		
		private ObjectHouse m_house;
		
		public Phony (Magnum _magnum, TileMap _map, Game _game, ObjectHouse _house) : base (_magnum, _map, _game)
		{
			m_house = _house;
			m_magnum.ShowCrosshair = false;
		}
		
		protected override void Control (float _delta, IEnumerable<Bumped> _bumps)
		{
			if (_bumps.Contains(Bumped.Left)) {
				m_direction = Vector2f.Right;
			} else if (_bumps.Contains(Bumped.Right)) {
				m_direction = Vector2f.Left;
			}
			
			if (m_direction.Equals(Vector2f.Left)) {
				GoLeft();
			} else if (m_direction.Equals(Vector2f.Right)) {
				GoRight();
			}
			
			var gangsters = m_house.GetAllGangsters();
			Gangster closeGangster = gangsters.First();
			float distance = float.MaxValue;
			foreach (var gangster in gangsters) {
				if (gangster == this) continue;
				float thisDistance = (gangster.Position - Position).LengthSquared();
				if (thisDistance < distance) {
					distance = thisDistance;
					closeGangster = gangster;
				}
			}
			
			if (distance < distToEnemySquared) {
				m_aim = (closeGangster.Position + m_aim)/2;
				m_magnum.AimAt(m_aim);
				m_magnum.Shoot();
			} else {
				m_magnum.AimAt(Position + m_direction);
			}
		}
	}
}
