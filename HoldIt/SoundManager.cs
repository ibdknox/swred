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
using HoldItCore;
using Microsoft.Xna.Framework;


namespace HoldIt.Sounds
{
    public class SoundManager
    {
        public static void Play(string curSound, bool loop)
        {
            Stream sound = typeof(Level).Assembly.GetManifestResourceStream("HoldItCore.Sounds." + curSound);
            SoundEffect effect = SoundEffect.FromStream(sound);
            SoundEffectInstance soundInstance = effect.CreateInstance();
            FrameworkDispatcher.Update();
            soundInstance.IsLooped = loop;
            soundInstance.Play();
        }
    }
}
