﻿using System;
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
using HoldItCore.Sounds;

namespace HoldIt
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            SoundManager.InitSoundSource(new XNASoundPlayer());

			this.PlayButton.Click += this.Start;

			this.Scores.Click += (sender, args) => this.NavigationService.Navigate(new Uri("/ScoreList.xaml", UriKind.Relative));
        }

		private void Start(object sender, EventArgs e) {
			this.NavigationService.Navigate(new Uri("/Levels.xaml", UriKind.Relative));
		}
    }
}