
using HoldItCore.Levels;
using System.Windows.Controls;
using System.Collections.Generic;
using System;

namespace HoldItCore {
	public partial class LevelContainer : UserControl {

		private int levelIndex = -1;
		private List<LevelInfo> levels = new List<LevelInfo>();
		private Level currentLevel = null;

		public LevelContainer() {
			InitializeComponent();

			this.levels.Add(new LevelInfo() {
				LevelType = typeof(Easy),
				Title = "Easy",
			});

			this.levels.Add(new LevelInfo() {
				LevelType = typeof(IntroLevel),
				Title = "Intro",
			});

			this.NextLevel();
		}

		private void NextLevel() {

			if (this.currentLevel != null) {
				this.currentLevel.Completed -= this.HandleLevelCompleted;
				this.LayoutRoot.Children.Remove(this.currentLevel);
			}

			++this.levelIndex;
			if (this.levelIndex < this.levels.Count) {
				this.currentLevel = (Level)Activator.CreateInstance(this.levels[this.levelIndex].LevelType);
				this.currentLevel.Completed += this.HandleLevelCompleted;

				this.LayoutRoot.Children.Add(this.currentLevel);
			}
		}

		private void HandleLevelCompleted(object sender, EventArgs e) {
			
			this.NextLevel();
		}
	}
}
