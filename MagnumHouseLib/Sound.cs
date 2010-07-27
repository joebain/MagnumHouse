using System;
using Tao.Sdl;
using SdlDotNet.Audio;

namespace MagnumHouseLib
{
	public class Sound
	{
		SdlDotNet.Audio.Sound sound;
		
		public Sound (string _filename)
		{
			sound = new SdlDotNet.Audio.Sound(_filename);
		}
		
		public void Play() {
			try {
				sound.Play();
			} catch {}
		}
	}
}
