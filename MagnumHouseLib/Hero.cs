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
		
		public Hero (UserInput _keys, Magnum _magnum, TileMap _tiles, Game _game) : base (_magnum, _tiles, _game)
		{
			m_keys = _keys;
			m_magnum.invulnerable = true;
			
			jumpSound = new Sound("sounds/jump.wav");
			landSound = new Sound("sounds/land.wav");
		}
		
		protected override void HitFloor ()
		{
			//landSound.Play();
		}
		
		protected override void Control(float _delta, IEnumerable<Bumped> _bumps) {
			
			//key input
			if (onFloor && groundTimeCount <= 0) {
				if (m_keys.IsKeyPressed(Sdl.SDLK_UP) || m_keys.IsKeyPressed(Sdl.SDLK_w)) {
					m_speed.Y = jumpSpeed;
					jumpSound.Play();
				}
			}
			if (m_keys.IsKeyPressed(Sdl.SDLK_RIGHT) || m_keys.IsKeyPressed(Sdl.SDLK_d)) {
				GoRight();
			}
			else if (m_keys.IsKeyPressed(Sdl.SDLK_LEFT) || m_keys.IsKeyPressed(Sdl.SDLK_a)) {
				GoLeft();
			}
			
			//camera
			m_game.viewOffset = new Vector2f((Game.Width*0.5f)-Position.X, (Game.Height*0.5f)-Position.Y);
				
			//deal with gun
			if (m_keys.IsMouseButtonPressed(Sdl.SDL_BUTTON_LEFT)) {
				m_magnum.Shoot();
			}
			m_magnum.AimAt(m_game.ScreenPxToGameCoords(m_keys.MousePos));
			//Console.WriteLine("mouse is at: " + m_keys.MousePos.ScreenToGameCoords() + " or " + m_keys.MousePos);
		}
	}
}
