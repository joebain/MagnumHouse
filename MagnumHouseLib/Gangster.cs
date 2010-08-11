using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Tao.Sdl;
using Tao.OpenGl;

namespace MagnumHouseLib
{
	public class Gangster : BumblingThing, IUpdateable, IDrawable, IShootable
	{
		protected Magnum m_magnum;
		
		public Magnum Magnum { get { return m_magnum; }}
		
		protected Vector2f m_acceleration = new Vector2f();
		public Vector2f Acceleration { get { return m_acceleration; }}
		protected Vector2f m_speed = new Vector2f();
		
		[Range(0, 30, 0)]
		public static float jumpSpeed = 17f;
		[Range(0, 30, 0)]
		public static float WalkSpeed = 27f;
		public virtual float walkSpeed { get { return WalkSpeed; }}
		[Range(0, 60, 0)]
		public static float Gravity = 41f;
		float gravity {get{return Gravity;}}//(1f/m_magnum.Size)*Gravity;}}
		[Range(0, 30, 0)]
		public static float MaxSpeed = 20.0f;
		public virtual float maxSpeed {get { return MaxSpeed; }}
		
		[RangeAttribute(0,30,1)]
		public static float maxXSpeed = 10.0f;
		[RangeAttribute(0,30,1)]
		public static float maxYSpeed = 20.0f;
		
		[RangeAttribute(0, 30, 0)]
		public static float MaxFloorSpeed = 5.0f;
		public virtual float maxFloorSpeed { get { return MaxFloorSpeed; }}
		[Range(0, 100, 0)]
		public static float Friction = 10f;
		public virtual float friction { get { return Friction; }}
		[Range(0, 2, 2)]
		public static float airTractionMultiplier = 1.0f;
		[Range(0, 2, 2)]
		public static float airFrictionMultiplier = 0.1f;
		[Range(0, 2, 2)]
		public static float walkSpeedSizeMult = 1.0f;
		[Range(0,20,1)]
		public static float wallSlideSpeed = 1.0f;
		
		protected const float size = 0.8f;
		
		[Range(0, 2, 2)]
		public static float groundTime = 0.2f;
		protected float groundTimeCount = 0;
		[Range(0,2,2)]
		public static float wallTime = 0.1f;
		protected float wallTimeCount = 0;
		protected double m_time = 0;
		
		protected bool onFloor = false;
		protected bool onWallRight = false;
		protected bool onWallLeft = false;
		protected IEnumerable<Bumped> bumps = new List<Bumped>();
			
		protected IObjectCollection m_house;
		
		public Layer Layer { get { return Layer.Pixelly; }}
		
		public Gangster (IObjectCollection _house)
		{
			m_magnum = new Magnum(_house);
			m_magnum.Owner = this;
			m_house = _house;
		}
		
		public override Vector2f Position {
			get { return m_position; }
			set { m_position = value; }
		}
		
		public override Vector2f Size { 
			get { return new Vector2f(size, size); }
			set {}
		}
		
		public override Vector2f Speed {
			get { return m_speed; }
			set { m_speed = value; }
		}
		
		protected virtual void Control(float _delta, IEnumerable<Bumped> _bumps) {
			
		}
		
		protected void GoRight() {
			if (onFloor) m_acceleration.X = walkSpeed * (walkSpeedSizeMult/m_magnum.Size);
				else m_acceleration.X = walkSpeed*airTractionMultiplier;
		}
		
		protected void GoLeft() {
			if (onFloor) m_acceleration.X = -walkSpeed * (walkSpeedSizeMult/m_magnum.Size);
				else m_acceleration.X = -walkSpeed*airTractionMultiplier;
		}
		
		public void Update(float _delta)
		{
			m_speed += m_acceleration*_delta;
			float tmpMaxSpeed;
			
			tmpMaxSpeed = maxSpeed;
			
			//collisions
			bumps = TryMove(m_speed*_delta);
			if (bumps.Contains(Bumped.Left)) {
				if (!onWallLeft) {
					HitWallLeft();
				}
				onWallLeft = true;
				m_speed.X = 0;
				if (m_speed.Y < 0) {
					tmpMaxSpeed = wallSlideSpeed;
				}
			} else {
				onWallLeft = false;
			}
			if (bumps.Contains(Bumped.Right)) {
				if (!onWallRight) {
					HitWallRight();
				}
				onWallRight = true;
				m_speed.X = 0;
				if (m_speed.Y < 0) {
					tmpMaxSpeed = wallSlideSpeed;
				}
			} else {
				onWallRight = false;
			}
			if (onWallLeft || onWallRight) {
				wallTimeCount -= _delta;	
			} else {
				wallTimeCount = wallTime;
			}
			if (bumps.Contains(Bumped.Bottom)) {
				m_speed.Y = 0;
				if (!onFloor) {
					HitFloor();
				}
				onFloor = true;
			} else {
				onFloor = false;
			}
			if (bumps.Contains(Bumped.Top)) {
				m_speed.Y = 0;
			}
			//PrintBumps(bumps);	
			
			m_speed.Cap(tmpMaxSpeed);
			if (onFloor) {
				m_speed.Cap(maxFloorSpeed, maxYSpeed);
			}
			else {
				m_speed.Cap(maxXSpeed, maxYSpeed);
			}
			
			//physics
			if (doPhysics) {
				m_acceleration.Y = -gravity;
				
				if (onFloor) {
					m_acceleration.X = -m_speed.X * friction;
					groundTimeCount -= _delta;
				} else {
					m_acceleration.X = -m_speed.X * friction * airFrictionMultiplier;
					groundTimeCount = groundTime;
				}
			}
			//Console.WriteLine("gt " + groundTimeCount);
				
			
//			Console.WriteLine("speed : " + m_speed);
//			Console.WriteLine("acceleration : " + m_acceleration);
//			Console.WriteLine("position : " + m_position);
			//m_time += _delta;
			//log.WriteLine(String.Format("{0}, {1}, {2}, {3}",m_time, m_position.CSV(), m_speed.CSV(), m_acceleration.CSV()));
			
			Control(_delta, bumps);
			
			m_magnum.Update(_delta);
		}
		
		protected virtual void HitFloor() {
			
		}
		
		protected virtual void HitWallLeft() {
			
		}
		
		protected virtual void HitWallRight() {
			
		}
		
		protected virtual void SetColour() {
			Gl.glColor3f(1, 0, 1);	
		}
		
		public void Draw()
		{
			Gl.glPushMatrix();
			Gl.glTranslatef(m_position.X, m_position.Y, 0);
			
			SetColour();
                
            Gl.glBegin(Gl.GL_TRIANGLES);
                Gl.glVertex3f( 0.0f, 0.0f, 0.0f);
                Gl.glVertex3f( size, 0.0f, 0.0f);
                Gl.glVertex3f( size*0.5f, size, 0.0f);
            Gl.glEnd();
			
			Gl.glPopMatrix();
			
			m_magnum.Draw();
		}
		
		public override bool Dead {get ; set;}
		
		public void GotShot(Slug _slug) {
			m_magnum.Smaller(_slug.Size.Length());
		}
	}
}
