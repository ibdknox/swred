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
using System.IO;
using System.Windows.Resources;

namespace HoldItCore.Sounds
{
    public class SoundManager
    {

        private static ISoundPlayer _player;
        public static void InitSoundSource(ISoundPlayer player)
        {
            _player = player;
        }

        public static void Play(string curSound, bool loop)
        {
            if (_player == null)
                return;

            _player.Play(curSound, loop);
        }
    }
}
