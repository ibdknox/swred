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

namespace HoldIt
{
	public partial class Settings : PhoneApplicationPage
	{
        public static int MusicVolume { get; private set; }
        public static int EffectsVolume { get; private set; }

        static Settings()
        {
            MusicVolume = 80;
            EffectsVolume = 65;
        }

		public Settings()
		{
			InitializeComponent();

            this.MusicVolumeSlider.ValueChanged += (sender, args) =>
                {
                    MusicVolume = (int)args.NewValue;
                    MusicVolumeLabel.Text = ((int)args.NewValue).ToString();
                };

            this.EffectsVolumeSlider.ValueChanged += (sender, args) =>
                {
                    EffectsVolume = (int)args.NewValue;
                    EffectsVolumeLabel.Text = ((int)args.NewValue).ToString();
                };

            MusicVolumeSlider.Value = MusicVolume;
            EffectsVolumeSlider.Value = EffectsVolume;
		}
	}
}