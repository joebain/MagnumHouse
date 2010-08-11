using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Tao.Sdl;
using Tao.OpenGl;

namespace MagnumHouseLib
{
	public class Hero : Gangster
	{
		private UserInput m_keys;
		
		Sound jumpSound;
		Sound landSound;
		
		bool justHitWall = false;
		
		public Hero (UserInput _keys, ObjectHouse _house) : base ( _house)
		{
			m_keys = _keys;
			m_magnum.invulnerable = true;
			
			jumpSound = new Sound("sounds/jump.wav");
			landSound = new Sound("sounds/land.wav");
		}
		
		protected override void HitFloor ()
		{
			landSound.Play();
		}
		
		protected override void HitWallLeft() {
			justHitWall = true;
		}
		
		protected override void HitWallRight() {
			justHitWall = true;
		}
		
		protected override void SetColour ()
		{
			Gl.glColor3f(0.977f, 0.438f, 0.422f);
			//Gl.glColor3f(0.977f,0f,0.422f);
		}
		
		protected override void Control(float _delta, IEnumerable<Bumped> _bumps) {
			
			//key input
			if (m_keys.IsKeyPressed(Sdl.SDLK_UP) || m_keys.IsKeyPressed(Sdl.SDLK_w)) {
				if (onFloor && groundTimeCount <= 0) {
					m_speed.Y = jumpSpeed;
					jumpSound.Play();
				}
				if (!justHitWall && !onFloor) {
					if (onWallLeft && wallTimeCount <= 0) {
						m_speed.Y = jumpSpeed;
						m_speed.X = jumpSpeed;
						jumpSound.Play();
					}
					if (onWallRight && wallTimeCount <= 0) {
						m_speed.Y = jumpSpeed;
						m_speed.X = -jumpSpeed;
						jumpSound.Play();
					}
				}
			} else if (!onFloor) {
				justHitWall = false;
			}
			
			
			if (m_keys.IsKeyPressed(Sdl.SDLK_RIGHT) || m_keys.IsKeyPressed(Sdl.SDLK_d)) {
				GoRight();
			}
			else if (m_keys.IsKeyPressed(Sdl.SDLK_LEFT) || m_keys.IsKeyPressed(Sdl.SDLK_a)) {
				GoLeft();
			}
				
			//deal with gun
			if (m_keys.IsMouseButtonPressed(Sdl.SDL_BUTTON_LEFT)) {
				m_magnum.Shoot();
			}
			m_magnum.AimAt(m_keys.MousePos);
			//Console.WriteLine("mouse is at: " + m_keys.MousePos.ScreenToGameCoords() + " or " + m_keys.MousePos);
		}
	}
}
