
using System;
using Tao.OpenGl;

namespace MagnumHouse
{
	public class Magnum : IDrawable, IUpdateable
	{

		float fireRate = 0.5f;
		
		float bulletCounter;
		
		public Vector2f Position = new Vector2f();
		public Vector2f Direction = Vector2f.Right;
		
		public float Size = 10f;
		[Range(0,2,2)]
		public static float sizeStep = 0.04f;
		[Range(0,2,2)]
		public static float fireRateSizeMultiplier = 0.2f;
		[Range(0,30,0)]
		public static float kickback = 3f;
		[Range(0,5,2)]
		public static float kickBackSizeMultiplier = 2.13f;
		[Range(0,100,0)]
		public static float maxSize = 4f;
		[Range(0,10,2)]
		public static float minSize = 0.2f;
		[Range(0,5,2)]
		public static float smallnessMultiplier = 2f;
		
		public bool ShowCrosshair = true;
		public bool invulnerable = false;
		
		Vector2f m_aim = new Vector2f();
		
		ObjectHouse m_house;
		private Thing2D m_owner;
		public Thing2D Owner {
			get {return m_owner;} 
			set {
				m_owner = value;
				Size = m_owner.Size.X/4;
				fireRate = fireRateSizeMultiplier * (1/Size);
			}
		}
		Sound fireSound;
		
		public Magnum (ObjectHouse _house)
		{
			m_house = _house;
			fireSound = new Sound("sounds/fire.wav");
			
		}
		
		public void Draw() {
			//gun
			Gl.glPushMatrix();
						
			Gl.glColor3f(0.5f,0,0);
			
			//Gl.glRotatef(Direction.Angle(), 0, 0, 1);
			Gl.glTranslatef(Position.X, Position.Y, 0);
			Gl.glRotatef(-Direction.Angle()*(float)(180f/Math.PI), 0, 0, 1);
			
			Gl.glBegin(Gl.GL_TRIANGLE_STRIP);
			Gl.glVertex2f(-Size,Size);	
			Gl.glVertex2f(-Size,0f);
			Gl.glVertex2f(Size,Size);	
			Gl.glVertex2f(Size,0f);
			Gl.glEnd();
			
			Gl.glPopMatrix();
			
			//target
			if (ShowCrosshair) {
				Gl.glPushMatrix();
				
				Gl.glColor3f(1.0f,1.0f,1.0f);
				Gl.glTranslatef(m_aim.X, m_aim.Y, 0);
				
				Gl.glBegin(Gl.GL_LINES);
				
				Gl.glVertex2f(-Size, -Size);
				Gl.glVertex2f(Size, Size);
				Gl.glVertex2f(-Size, Size);
				Gl.glVertex2f(Size, -Size);
				
				Gl.glEnd();
				
				Gl.glPopMatrix();
			}
		}
		
		public void Update(float _delta) {
			if (bulletCounter > 0) bulletCounter -= _delta*fireRate;
			
			if (m_owner != null) {
				Position = m_owner.Position + Direction + Vector2f.Up*m_owner.Size.Y*0.5f;
				if (m_owner.Dead) Dead = true;
			}
		}
		
		public void Shoot() {
			if (bulletCounter <= 0) {
				fireSound.Play();
				bulletCounter = 0.1f;
				Slug slug = new Slug(this);
				slug.Position = Position + Direction * slug.Size.Length();
				slug.Direction = Direction;
				m_house.AddDrawable(slug);
				m_house.AddUpdateable(slug);
				m_owner.Speed += Direction *-kickback  * kickBackSizeMultiplier * Size;
			}
		}
		
		public void AimAt(Vector2f _aim) {
			m_aim = _aim;
			Direction = _aim - m_owner.Position;
			Direction.Normalise();
		}
		
		public void MadeAKill(IShootable _killee) {
			Bigger();
		}
		
		public void Bigger() {
			if (Size < maxSize)
				Size += sizeStep;
			fireRate = fireRateSizeMultiplier * (1/Size);
		}
		
		public void Smaller(float _amount) {
			if (Size > minSize) {
				Size -= sizeStep * _amount * smallnessMultiplier;
				fireRate = (1/Size) * fireRateSizeMultiplier;
			} else {
				if (!invulnerable) {
					m_owner.Dead = true;
					Dead = true;
				}
			}
		}
		
		public bool Dead {get; private set;}
	}
}
