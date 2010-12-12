using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using System.Windows.Resources;
using HoldItCore.Sounds;
using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace HoldIt
{
    public class XNASoundPlayer : ISoundPlayer
    {

        private Dictionary<String, SoundEffectInstance> _sounds;

        public XNASoundPlayer()
        {
            _sounds = new Dictionary<string, SoundEffectInstance>();
        }

        public Action Play(string curSound, bool loop)
        {
            Stream sound = typeof(SoundManager).Assembly.GetManifestResourceStream("HoldItCore.Sounds." + curSound);
            SoundEffect effect = SoundEffect.FromStream(sound);
            SoundEffectInstance soundInstance = effect.CreateInstance();
            FrameworkDispatcher.Update();
            soundInstance.IsLooped = loop;
            soundInstance.Play();

			return () => {
				if (soundInstance.IsDisposed)
				{
					soundInstance.Stop();
				}
			};
        }
    }
}
