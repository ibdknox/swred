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
using HoldItCore.Sounds;
using HoldItCore;
using System.Collections.ObjectModel;
using System.Windows.Navigation;

namespace HoldIt
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
			this.Levels = new ObservableCollection<LevelInfo>();

            InitializeComponent();

			this.DataContext = this;

            SoundManager.InitSoundSource(new XNASoundPlayer());



			//this.Scores.Click += (sender, args) => this.NavigationService.Navigate(new Uri("/ScoreList.xaml", UriKind.Relative));
			//this.Settings.Click += (sender, args) => this.NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
        }

		public ObservableCollection<LevelInfo> Levels { get; set; }

		private void Start(object sender, EventArgs e) {
			this.NavigationService.Navigate(new Uri("/Levels.xaml", UriKind.Relative));
		}
		private void HandleShowScores(object sender, EventArgs e) {
			this.NavigationService.Navigate(new Uri("/ScoreList.xaml", UriKind.Relative));
		}

		private void HandleShowSettings(object sender, EventArgs e) {
			this.NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
		}

		private void HandleSelectionChanged(object sender, SelectionChangedEventArgs e) {
			if (e.AddedItems.Count == 1) {
				LevelInfo levelInfo = e.AddedItems[0] as LevelInfo;

				if (levelInfo != null) {
					if (this.NavigationService != null)
						this.NavigationService.Navigate(new Uri("/Levels.xaml?Level="+ levelInfo.Title, UriKind.Relative));
				}
			}
		}

		protected override void OnNavigatedTo(NavigationEventArgs e) {
			base.OnNavigatedTo(e);

			this.Levels.Clear();

			foreach (LevelInfo level in LevelInfo.AllLevels)
				this.Levels.Add(level);
		}
    }
}