
using System;

namespace MagnumHouseLib
{
	[Serializable]
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
		
		public static Colour Parse(string text) {
			Colour colour = new Colour();
			float[] fVals = new float[4];
			string[] components = text.Split(new []{','});
			if (components.Length != 4) return colour;
			int i = 0;
			foreach (string component in components) {
				int iVal = int.Parse(component);
				if (iVal < 0 | iVal > 255) return colour;
				fVals[i] = iVal / 255.0f;
				i ++;
			}
			colour.R = fVals[0];
			colour.G = fVals[1];
			colour.B = fVals[2];
			colour.A = fVals[3];
			return colour;
		}
		
		public override string ToString ()
		{
			return String.Format("{0},{1},{2},{3}", (int)(R * 255), (int)(G*255), (int)(B*255), (int)(A*255));
		}
	}
}