﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace HoldItCore
{
	public class SettingsStore
	{
		static SettingsStore()
        {
            SettingsStore.MusicVolume = 80;
            SettingsStore.EffectsVolume = 65;
        }

		public static int MusicVolume { get; set; }
		public static int EffectsVolume { get; set; }
	}
}
