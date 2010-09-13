using System;

namespace MagnumHouseLib
{
	public class FadeEvent : Event
	{
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
		
		public FadeEvent (Camera camera, Colour start, Colour end, float duration, Trigger trigger, ObjectHouse house) : base(trigger)
		{
			m_house = house;
			fx_buffer.SetHUD(camera);
			fx_buffer.SetFading(1f/duration, start, end);
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
	}
}
