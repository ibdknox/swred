using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using HoldItCore;

namespace HoldIt
{
	public partial class Settings : PhoneApplicationPage
	{

        static Settings()
        {
            SettingsStore.MusicVolume = 80;
            SettingsStore.EffectsVolume = 65;
        }

		public Settings()
		{
			InitializeComponent();

            this.MusicVolumeSlider.ValueChanged += (sender, args) =>
                {
                    SettingsStore.MusicVolume = (int)args.NewValue;
                    MusicVolumeLabel.Text = ((int)args.NewValue).ToString();
                };

            this.EffectsVolumeSlider.ValueChanged += (sender, args) =>
                {
                    SettingsStore.EffectsVolume = (int)args.NewValue;
                    EffectsVolumeLabel.Text = ((int)args.NewValue).ToString();
                };

            MusicVolumeSlider.Value = SettingsStore.MusicVolume;
            EffectsVolumeSlider.Value = SettingsStore.EffectsVolume;
		}
	}
}