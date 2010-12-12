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
using System.Text;
using HoldItCore;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Json;

namespace HoldIt
{
	public partial class ScoreList : PhoneApplicationPage
	{
		static string baseUrl = "http://holditscores.appspot.com";

		delegate void DownloadComplete(string content);
		delegate void DownloadFailed(Exception e);

        List<VisualScoreInfo> highScoreList = new List<VisualScoreInfo>();
        List<VisualScoreInfo> myScoreList = new List<VisualScoreInfo>();

		abstract class VisualScoreInfo
		{
            protected VisualScoreInfo(ScoreData score, int sortValue) { this.Score = score; this.SortValue = sortValue; }
			public ScoreData Score { get; private set; }
            public int SortValue { get; private set; }
            abstract public UIElement ToVisual();

		}

        class LevelScoreTextBox : VisualScoreInfo
		{
            public LevelScoreTextBox(ScoreData score)
                : base(score, score.LevelIndex)
            { }

            public override UIElement ToVisual()
            {
                return new TextBlock() { Text = string.Format("'{0}': ranked #{1} - {2} points", Score.Level, Score.Rank, Score.Score) };
            }
		}

        class HighScoreTextBox : VisualScoreInfo
		{
            public HighScoreTextBox(ScoreData score)
                : base(score, -score.Rank)
            { }

            public override UIElement ToVisual()
            {
                return new TextBlock() { Text = string.Format("#{0} - {1}, {2} points", Score.Rank, Score.Player, Score.Score) };
            } 
		}

		[DataContract]
		public class ScoreData
		{
			[DataMember]
			public string Player { get; set; }
			[DataMember]
			public int Score { get; set; }
			[DataMember]
			public int Rank { get; set; }
			[DataMember]
			public string Level { get; set; }

			public int LevelIndex { get; set; }
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

				DownloadAsync("/getrank", parameters,
					onComplete: output => InsertMyScore(ParseOutputIntoScore(output, i, level)),
					onError: error => Debug.WriteLine(error.ToString()));
			}

            // Kick off downloads for high scores
            //for (int i = 0; i < Level.LevelNames.Count; i++)
            {
                var level = "Intro";
            
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("level", level);
            
                DownloadAsync("/listscores", parameters,
                    onComplete: output => 
                        {
                            foreach (var score in ParseOutputIntoScores(output, 0, level))
                            {
                                InsertHighScore(score);
                            }
                        },
                    onError: error => Debug.WriteLine(error.ToString()));
            }
		}

		ScoreData ParseOutputIntoScore(string output, int levelIndex, string level)
		{
			ScoreData score;
			using( MemoryStream ms = new MemoryStream( Encoding.Unicode.GetBytes( output ) ) )
			{
			    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ScoreData));

				score = (ScoreData)serializer.ReadObject(ms);
			}

			score.LevelIndex = levelIndex;

			return score;
		}

        IEnumerable<ScoreData> ParseOutputIntoScores(string output, int levelIndex, string level)
        {
            using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(output)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ScoreData[]));

                while (ms.Position < ms.Length)
                {
                    ScoreData[] scores = (ScoreData[])serializer.ReadObject(ms);

                    foreach (var score in scores)
                    {
                        score.LevelIndex = levelIndex;
                        yield return score;
                    }
                }
            }
        }

        void InsertScore(VisualScoreInfo boxWithScore, List<VisualScoreInfo> scores, UIElementCollection list)
		{
			for (int i = 0; i < scores.Count; i++)
			{
				var item = scores[i];
				if (boxWithScore.SortValue > item.SortValue)
				{
					list.Insert(i, boxWithScore.ToVisual());
                    scores.Insert(i, boxWithScore);
					return;
				}
			}
			// If we hit this point, it goes at the end
			list.Add(boxWithScore.ToVisual());
            scores.Add(boxWithScore);
		}

		void InsertHighScore(ScoreData score)
		{
			InsertScore(new HighScoreTextBox(score), this.highScoreList, this.HighScoreList.Children);
		}

		void InsertMyScore(ScoreData score)
		{
			InsertScore(new LevelScoreTextBox(score), this.myScoreList, this.MyScoreList.Children);
		}

		void DownloadAsync(string page, Dictionary<string, string> parameters, DownloadComplete onComplete, DownloadFailed onError)
		{
			WebClient client = new WebClient();

			client.DownloadStringCompleted += (sender, args) =>
				{
					if (args.Error != null)
						onError(args.Error);
					else
						onComplete(args.Result);
				};

			// Generate GET variables
			StringBuilder vars = new StringBuilder();
			foreach (var param in parameters)
				vars.Append(string.Format("{0}={1}&", param.Key, param.Value));

			Uri url = new Uri(baseUrl + page + "?" + vars.ToString());

			client.DownloadStringAsync(url);
		}

	}
}