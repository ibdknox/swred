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
using System.Diagnostics;

namespace HoldIt
{
    public partial class LevelHighScoreList : PhoneApplicationPage
    {
        public LevelHighScoreList()
        {
            InitializeComponent();
        }

        List<VisualScoreInfo> ScoreData = new List<VisualScoreInfo>();

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (ScoreData.Count > 0)
                return;

            string level;
            string player;
            if (this.NavigationContext.QueryString.TryGetValue("level", out level))
            {
                this.GenerateScoresForLevel(level);
            }
            else if (this.NavigationContext.QueryString.TryGetValue("player", out player))
            {
                this.GenerateScoresForPlayer(player);
            }
        }

        void GenerateScoresForPlayer(string player)
        {
            this.PageTitle.Text = "Player: " + player;

            for (int i = 0; i < Level.LevelNames.Count; i++)
            {
                var level = Level.LevelNames[i];

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("name", player);
                parameters.Add("level", level);

                ScoreUtil.DownloadAsync("/getscorebyname", parameters,
                    onComplete: output => 
                        {
                            var score = ScoreUtil.ParseOutputIntoScore(output, i, level);
                            ScoreUtil.InsertScore(new LevelScoreTextBox(score), ScoreData, this.Scores.Items);
                        },
                    onError: error => Debug.WriteLine(error.ToString()));
            }

            this.Scores.SelectionChanged += (sender, args) =>
            {
                int index = this.Scores.SelectedIndex;

                string level = this.ScoreData[index].Score.Level;
                this.NavigationService.Navigate(new Uri("/HighScoreList.xaml?level=" + level, UriKind.Relative));
            };
        }

        void GenerateScoresForLevel(string level)
        {
            this.PageTitle.Text = "Level: " + level;

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("level", level);

            ScoreUtil.DownloadAsync("/listscores", parameters,
                onComplete: output =>
                {
                    foreach (var score in ScoreUtil.ParseOutputIntoScores(output, 0, level))
                    {
                        ScoreUtil.InsertScore(new HighScoreTextBox(score), ScoreData, this.Scores.Items);
                    }
                },
                onError: error => Debug.WriteLine(error.ToString()));


            this.Scores.SelectionChanged += (sender, args) =>
            {
                int index = this.Scores.SelectedIndex;

                string player = this.ScoreData[index].Score.Player;
                this.NavigationService.Navigate(new Uri("/HighScoreList.xaml?player=" + player, UriKind.Relative));
            };
        }
    }
}