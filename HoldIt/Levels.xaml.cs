
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using HoldItCore;
using System;

namespace HoldIt {
	public partial class Levels : PhoneApplicationPage {

		private Level currentLevel = null;

		public Levels() {
			InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e) {
			base.OnNavigatedTo(e);

			string levelName = null;
			this.NavigationContext.QueryString.TryGetValue("Level", out levelName);

			LevelInfo selectedLevel = null;
			foreach (LevelInfo levelInfo in LevelInfo.AllLevels) {
				if (levelInfo.Title == levelName)
					selectedLevel = levelInfo;
			}

			this.currentLevel = (Level)Activator.CreateInstance(selectedLevel.LevelType);

			this.currentLevel.Completed += this.HandleLevelCompleted;

			this.LayoutRoot.Children.Add(this.currentLevel);
		}

		private void HandleLevelCompleted(object sender, EventArgs e) {
			this.NavigationService.GoBack();
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e) {
			if (this.currentLevel != null) {
				this.currentLevel.Stop();
				this.currentLevel = null;
			}
		}
	}
}