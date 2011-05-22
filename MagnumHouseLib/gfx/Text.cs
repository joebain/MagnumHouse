
using System;
using System.Drawing;
using System.Collections.Generic;

namespace MagnumHouseLib
{
	class Character {
		public Vector2f Size;
		public Vector2f Position;
		public char Char;
	}
	
	public class Text : Thing2D, IDrawable, IUpdateable
	{
		public enum PointSizes {
			Ten = 10, Sixteen = 16, TwentyTwo = 22
		}
		
		static Dictionary<char, Character> s_characters = new Dictionary<char, Character>();
		static Sprite s_sprite;
		static Bitmap s_tmpBmp = new Bitmap(1024, 1024);
		static Font s_font = new Font(FontFamily.GenericMonospace, (int)PointSizes.TwentyTwo);
		static Vector2f s_totalSize = new Vector2f();
		public static readonly string characters =
				"abcdefghijklmnopqrstuvwxyz" +
				"ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
				"0123456789" +
				",.!?\"' :-_=+*/\\][{}()@;";
		
		public Layer Layer { get;set;}
		public Priority Priority {get ;set;}
		public Colour Colour{get;set;}
		public bool Hidden = false;
		
		public int PointSize = (int)PointSizes.TwentyTwo;
		
		string m_text = "";
		public string Contents {
			get { return m_text;}
			set {
				m_text = value;
				SetSize();
			}
		}
		
		public float Transparency { get { return Colour.A;} set { Colour.A = value; }}
		
		public Text () : this("") {
		}
		
		public Text(string _text) {
			Contents = _text;
			Layer = Layer.Normal;
			Priority = Priority.Middle;
			Colour = new Colour(){A = 1};
			SetSize();
		}
		
		private void SetSize() {
			Size = Vector2f.Zero;
			foreach (char letter in Contents) {
				var charSize = s_characters[letter].Size;
				Size.X += charSize.X;
				if (charSize.Y > Size.Y) Size.Y = charSize.Y;
			}
		}
		
		public static void Init() {
			s_sprite = new Sprite(s_tmpBmp);
			s_totalSize = new Vector2f(s_sprite.Bitmap.Size.Width, s_sprite.Bitmap.Size.Height)/Tile.Size;
			
			Vector2f worldPosition = new Vector2f(0,0);
			Vector2f pixelPosition = new Vector2f(0,0);
			Vector2f previousPixelPosition = new Vector2f(0,0);
			Vector2f pixelSize = new Vector2f(0,0);
			Vector2f worldSize = new Vector2f(0,0);
			foreach (char character in characters) {
				previousPixelPosition = pixelPosition.Clone();
				pixelPosition = RenderCharacter (character, pixelPosition, out pixelSize);
				if (pixelPosition.X >= s_sprite.Bitmap.Size.Width) {
					pixelPosition.Y += pixelSize.Y;
					pixelPosition.X = 0;
					previousPixelPosition = pixelPosition.Clone();
					pixelPosition = RenderCharacter(character, pixelPosition, out pixelSize);
				}
				worldSize = pixelSize / Tile.Size;
				worldPosition = previousPixelPosition / Tile.Size;
				s_characters[character] = new Character() {
					Size = worldSize.Clone(),
					Position = worldPosition.Clone(),
					Char = character
				};
			}
			s_sprite.ReloadBitmap();
		}
		
		private static Vector2f RenderCharacter (char character, Vector2f pixelPosition, out Vector2f size)
		{
			var graphics = Graphics.FromImage(s_sprite.Bitmap);
			graphics.DrawString(
			                    character.ToString(), 
			                    s_font, 
			                    Brushes.White, 
			                    new PointF(pixelPosition.X, pixelPosition.Y));
			SizeF psize = graphics.MeasureString(character.ToString(), s_font);
			size = new Vector2f(psize.Width, psize.Height);
			pixelPosition.X += size.X;
			
			return pixelPosition;
		}
		
		public void Draw() {
			if (Hidden) return;
			
			s_sprite.Transparency = Colour.A;
			Character character;
			Vector2f drawPos = Position.Clone();
			foreach (char letter in Contents) {
				if (s_characters.TryGetValue(letter, out character)) {
					s_sprite.Position = drawPos;
					Vector2f size = (character.Size/(int)PointSizes.TwentyTwo)*PointSize;
					s_sprite.Size = size;
					s_sprite.CropNear = character.Position / s_totalSize;
					s_sprite.CropFar = (character.Position + character.Size) / s_totalSize;
					float tmp = s_sprite.CropNear.Y;
					s_sprite.CropNear.Y = 1 - s_sprite.CropFar.Y;
					s_sprite.CropFar.Y = 1 - tmp;
					s_sprite.Draw();
					//Console.WriteLine("drawing {4} ({5}) at {0}, size {1}, crop {2} and {3}",s_sprite.Position, s_sprite.Size,s_sprite.CropNear, s_sprite.CropFar, letter, character.Char);
					
					drawPos.X += size.X;
				}
			}
		}
		
		public Action<float> updateAction;
		
		public void Update(float _delta) {
			if (updateAction != null) updateAction(_delta);
		}
	}
}
