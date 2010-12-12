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

		class TextBoxWithScore : TextBox
		{
			protected TextBoxWithScore(ScoreData score) { this.Score = score; }
			public ScoreData Score { get; private set; }
		}

		class LevelScoreTextBox : TextBoxWithScore
		{
			public LevelScoreTextBox(ScoreData score) : base(score)
			{
				this.Text = string.Format("'{0}': #{0} - {1} points", score.Level, score.Rank, score.Score);
			}
		}

		class HighScoreTextBox : TextBoxWithScore
		{
			public HighScoreTextBox(ScoreData score) : base(score)
			{
				this.Text = string.Format("#{0} - {1}, {2} points", score.Rank, score.Player, score.Score);
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
				parameters.Add("player", "noah");
				parameters.Add("level", level);

				DownloadAsync("/getrank", parameters,
					onComplete: output => InsertHighScore(ParseOutputIntoScore(output, i, level)),
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

		void InsertScore(TextBoxWithScore boxWithScore, UIElementCollection list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				var item = list[i] as LevelScoreTextBox;
				if (boxWithScore.Score.LevelIndex > boxWithScore.Score.LevelIndex)
				{
					list.Insert(i, boxWithScore);
					return;
				}
			}
			// If we hit this point, it goes at the end
			list.Add(boxWithScore);
		}

		void InsertHighScore(ScoreData score)
		{
			InsertScore(new HighScoreTextBox(score), this.HighScoreList.Children);
		}

		void InsertMyScore(ScoreData score)
		{
			InsertScore(new LevelScoreTextBox(score), this.MyScoreList.Children);
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