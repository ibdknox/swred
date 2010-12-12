using System;
using System.Collections.Generic;
using System.Diagnostics;
using HoldItCore;
using Microsoft.Phone.Controls;

namespace HoldIt
{
	public partial class ScoreList : PhoneApplicationPage
	{
        List<VisualScoreInfo> HighScoreData = new List<VisualScoreInfo>();
        List<VisualScoreInfo> MyScoreData = new List<VisualScoreInfo>();


        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.HighScoreList.SelectedItem = null;
            this.MyScoreList.SelectedItem = null;
        }

		public ScoreList()
		{
			InitializeComponent();

			// Kick off downloads for my scores
			for (int i = 0; i < Level.LevelNames.Count; i++)
			{
				var level = Level.LevelNames[i];

				Dictionary<string, string> parameters = new Dictionary<string, string>();
				parameters.Add("name", "noah");
				parameters.Add("level", level);

                ScoreUtil.DownloadAsync("/getscorebyname", parameters,
                    onComplete: output => InsertMyScore(ScoreUtil.ParseOutputIntoScore(output, i, level)),
					onError: error => Debug.WriteLine(error.ToString()));
			}

            // Kick off downloads for high scores
            for (int i = 0; i < Level.LevelNames.Count; i++)
            {
                var level = Level.LevelNames[i];

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("rank", "1");
                parameters.Add("level", level);

                ScoreUtil.DownloadAsync("/getscorebyrank", parameters,
                    onComplete: output => InsertHighScore(ScoreUtil.ParseOutputIntoScore(output, i, level)),
                    onError: error => Debug.WriteLine(error.ToString()));
            }

            this.MyScoreList.SelectionChanged += (sender, args) =>
                {
                    int index = this.MyScoreList.SelectedIndex;
					if (index >= 0 && index < this.MyScoreData.Count) {

						string level = this.MyScoreData[index].Score.Level;
						this.NavigationService.Navigate(new Uri("/HighScoreList.xaml?level=" + level, UriKind.Relative));
					}
                };

            this.HighScoreList.SelectionChanged += (sender, args) =>
            {
                int index = this.HighScoreList.SelectedIndex;

				if (index >= 0 && index < this.MyScoreData.Count) {

					string level = this.HighScoreData[index].Score.Level;
					// Navigate to page for level
					this.NavigationService.Navigate(new Uri("/HighScoreList.xaml?level=" + level, UriKind.Relative));
				}
            };
		}


		void InsertHighScore(ScoreData score)
		{
			ScoreUtil.InsertScore(new LevelScoreTextBox(score), this.HighScoreData, this.HighScoreList.Items);
		}

		void InsertMyScore(ScoreData score)
		{
            ScoreUtil.InsertScore(new LevelScoreTextBox(score), this.MyScoreData, this.MyScoreList.Items);
		}

	}
}