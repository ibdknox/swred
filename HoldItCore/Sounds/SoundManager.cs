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
using System.Collections.Generic;

namespace HoldItCore.Sounds
{
    public class SoundManager
    {


		private static Dictionary<int, Action> stops = new Dictionary<int, Action>();
		private static int counter = 0;

        private static ISoundPlayer _player;
        public static void InitSoundSource(ISoundPlayer player)
        {
            _player = player;
        }

        public static int Play(string curSound, int volume, bool loop)
        {
            if (_player == null)
				return -1;

			counter++;
			stops[counter] = _player.Play(curSound, volume, loop);

			return counter;
        }

		public static void Stop(int index)
		{
			if (index < 0 || !stops.ContainsKey(index))
				return;

			stops[index]();
			stops.Remove(index);
		}

		public static void StopAll()
		{
			foreach (var index in stops.Keys)
			{
				stops[index]();
			}
			stops.Clear();
		}
    }
}
