
using System;
using Gdk;
using Gtk;
using System.Threading;

namespace MagnumHouse
{
	public partial class PhysicsAdjuster : Gtk.Window
	{
		const int statWidth = 200;
		const int statHeight = 200;
		private Point[] speedPoints = new Point[statWidth];
		private int speedPointI = 0;
		private Point[] accelPoints = new Point[statHeight];
		private int accelPointI = 0;
		
		bool stopStatUpdate = false;
		
		public PhysicsAdjuster () : base(Gtk.WindowType.Toplevel)
		{
			this.Build ();
			
			AddTabAndSlidersFor(typeof(Gangster));
			AddTabAndSlidersFor(typeof(Magnum));
			notebook.ShowAll();
			
			
			drawBBsToggleButton.Active = ObjectHouse.drawBoundingBoxes;
			
			statsArea.DoubleBuffered = true;
			
			GLib.Timeout.Add (100, new GLib.TimeoutHandler (UpdateStats));
		}
		
		private void AddTabAndSlidersFor(Type t) {
			Table table = new Table(1,1,false);
			notebook.AppendPage(table, new Label(t.Name));
			uint fieldCount = 0;
			foreach (var field in t.GetFields()) {
				if (field.FieldType == typeof(float) &&
				    field.IsPublic && field.IsStatic) {
					HScale hscale = new HScale(0, 1, 0.01);
					hscale.Digits = 2;
					foreach (var attribute in field.GetCustomAttributes(false)) {
						if (attribute is RangeAttribute) {
							var range = (RangeAttribute)attribute;
							hscale.Adjustment.Upper = range.m_max;
							hscale.Adjustment.Lower = range.m_min;
							hscale.Digits = range.m_places;
							break;
						}
					}
					hscale.Value = (float)field.GetValue(null);
					var localField = field;
					hscale.ValueChanged += (obj, args) => {
						localField.SetValue(null, (float)hscale.Value);
					};
					
					table.Resize(fieldCount+1, 2);
					Label label = new Label(field.Name);
					table.Attach(label , 0, 1, fieldCount, fieldCount+1);
					table.Attach(hscale, 1, 2, fieldCount, fieldCount+1);
					table.Homogeneous = false;
					table.ShowAll();
					fieldCount++;
				}
			}
		}
		
		public Gangster ParticularGangster;
		public Game ParticularGame;
		
		protected virtual void plusSizeButtonClicked (object sender, System.EventArgs e)
		{
			if (ParticularGangster != null) ParticularGangster.Magnum.Bigger();
		}
		
		protected virtual void minusSizeButtonClicked (object sender, System.EventArgs e)
		{
			if (ParticularGangster != null) ParticularGangster.Magnum.Smaller(1);
		}
		
		private bool UpdateStats() {
			
			if (ParticularGangster == null) return true;
			
			var speed = ((ParticularGangster.Speed / Gangster.MaxSpeed) * statHeight * 0.5f);
			speedPoints[speedPointI++] = new Point(speedPointI, (int)Math.Round(speed.Length()));
			if (speedPointI >= speedPoints.Length) speedPointI = 0;
			
			var accel = (ParticularGangster.Acceleration) * statHeight *0.3f;
			accelPoints[accelPointI++] = new Point(accelPointI, (int)Math.Round(accel.Length()));
			if (accelPointI >= accelPoints.Length) accelPointI = 0;
			
			
			if (statsArea.Visible)
				statsArea.QueueDraw();
			
			return !stopStatUpdate;
		}
		
		protected virtual void drawStatsArea (object o, Gtk.ExposeEventArgs args)
		{
			Gdk.GC red = new Gdk.GC (statsArea.GdkWindow);
			red.RgbFgColor = new Gdk.Color (255, 0, 0);
			Gdk.GC blue = new Gdk.GC (statsArea.GdkWindow);
			blue.RgbFgColor = new Gdk.Color (0, 0, 255);

			statsArea.GdkWindow.DrawPoints(blue, speedPoints);
			statsArea.GdkWindow.DrawPoints(red, accelPoints);
		}
		
		protected virtual void drawBoundingBoxesToggled (object sender, System.EventArgs e)
		{
			ObjectHouse.drawBoundingBoxes = drawBBsToggleButton.Active;
		}
		
		protected virtual void restartButtonClicked (object sender, System.EventArgs e)
		{
			if (ParticularGame != null) ParticularGame.Restart();
		}
	}
}
