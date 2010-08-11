using System;
using System.Linq;
using System.Collections.Generic;
using Tao.OpenGl;

namespace MagnumHouseLib
{


	public class EasyPhony : Gangster
	{

		Vector2f m_direction = Vector2f.Left;
		
		Vector2f m_aim = new Vector2f();
		float timeToFire = 1.0f;
		float fireCounter = 0f;
		
		private const float distToEnemySquared = 40f;
		
		public override float maxFloorSpeed {
			get {
				return MaxFloorSpeed/5;
			}
		}
		
		IGangsterProvider m_provider;
		
		public EasyPhony (IObjectCollection _house, IGangsterProvider _provider) : base (_house)
		{
			m_provider = _provider;
			m_magnum.ShowCrosshair = false;
		}
		
		protected override void SetColour ()
		{
			Gl.glColor3f(0.648f, 0.375f, 0.613f);
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
			
			var gangsters = m_provider.GetAllGangsters();
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
			fireCounter -= _delta;
			float angleToGangster = (closeGangster.Position - Position).Angle();
			if (distance < distToEnemySquared &&
			    closeGangster.Position.Y >= Position.Y && (closeGangster.Position.X - Position.X) * Math.Sign(m_direction.X) > 0) {
				m_aim = (closeGangster.Position + m_aim)/2;
				m_magnum.AimAt(m_aim);
				if (fireCounter <= 0 ) {
					m_magnum.Shoot();
					fireCounter = timeToFire;
				}
			} else {
				m_magnum.AimAt(Position + m_direction);
			}
		}
	}
}
