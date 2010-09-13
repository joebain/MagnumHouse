
using System;

namespace MagnumHouseLib
{
	public class Colour
	{

		public float R, G, B, A;
		
		public Colour (float r, float g, float b, float a)
		{
			R = r;
			G = g;
			B = b;
			A = a;
		}
		
		public Colour() {
			
		}
		
		public float[] Array {get{return new float[] {R, G, B, A};}}
		
		public static Colour operator * (Colour colour, float factor) {
			return new Colour(colour.R * factor, colour.G * factor, colour.B * factor, colour.A * factor);
		}
		
		public static Colour operator + (Colour one, Colour two) {
			return new Colour(one.R + two.R, one.G + two.G, one.B + two.B, one.A + two.A);
		}
		
		public static Colour Blend(Colour one, Colour two, float level) {
			return one*(1-level) + two*level;
		}
	}
}
