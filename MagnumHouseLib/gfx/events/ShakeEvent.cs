
using System;

namespace MagnumHouseLib
{
	public class ShakeEvent : Event
	{
		Random rand = new Random();
		
		Effect fx_buffer
				= new Effect(
				    new Vector2i(Game.ScreenWidth, Game.ScreenHeight),
					new Vector2i(Game.ScreenWidth, Game.ScreenHeight),
				    new Vector2i(Game.Width, Game.Height)) { 
				CaptureLayer = Layer.Normal,
				Layer = Layer.FX,
				Priority = Priority.Front
			};
		
		public Layer Layer {get{return fx_buffer.Layer;}}
		public Priority Priority{get {return fx_buffer.Priority;}}
		
		ObjectHouse m_house;
		bool m_started = false;
		float m_shakiness;
		
		public ShakeEvent (Camera camera, float shakiness, Trigger trigger, ObjectHouse house) : base(trigger)
		{
			m_house = house;
			fx_buffer.SetHUD(camera);
			m_shakiness = shakiness;
		}
		
		public override void Start() {
			m_house.AddUpdateable(fx_buffer);
			m_house.AddDrawable(fx_buffer);
			m_started = true;
		}
		
		public override void Stop ()
		{
			fx_buffer.Die();
			m_started = false;
		}
		
		public override void Update (float delta)
		{
			base.Update (delta);
			if (m_started)
				fx_buffer.Move(new Vector2f
				               ((float)rand.NextDouble()*0.1f*m_shakiness,
				                (float)rand.NextDouble()*0.1f*m_shakiness));
		}
	}
}
