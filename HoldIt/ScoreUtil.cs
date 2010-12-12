using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows.Controls;

namespace HoldIt
{

    delegate void DownloadComplete(string content);
    delegate void DownloadFailed(Exception e);

    static class ScoreUtil
    {
        static string baseUrl = "http://holditscores.appspot.com";


        public static ScoreData ParseOutputIntoScore(string output, int levelIndex, string level)
        {
            ScoreData score;
            using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(output)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ScoreData));

                score = (ScoreData)serializer.ReadObject(ms);
            }

            score.LevelIndex = levelIndex;

            return score;
        }

        public static IEnumerable<ScoreData> ParseOutputIntoScores(string output, int levelIndex, string level)
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

        public static void InsertScore(VisualScoreInfo boxWithScore, List<VisualScoreInfo> scores, ItemCollection list)
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

        public static void DownloadAsync(string page, Dictionary<string, string> parameters, DownloadComplete onComplete, DownloadFailed onError)
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

    abstract class VisualScoreInfo
    {
        protected VisualScoreInfo(ScoreData score, int sortValue) { this.Score = score; this.SortValue = sortValue; }
        public ScoreData Score { get; private set; }
        public int SortValue { get; private set; }
        abstract public ListBoxItem ToVisual();
    }

    class LevelScoreTextBox : VisualScoreInfo
    {
        public LevelScoreTextBox(ScoreData score)
            : base(score, score.LevelIndex)
        { }

        public override ListBoxItem ToVisual()
        {
            return new ListBoxItem()
            {
                Content = new TextBlock()
                {
                    Text = string.Format("'{0}': ranked #{1} - {2} points",
                                         Score.Level, Score.Rank, Score.Score)
                }
            };
        }
    }

    class HighScoreTextBox : VisualScoreInfo
    {
        public HighScoreTextBox(ScoreData score)
            : base(score, -score.Rank)
        { }

        public override ListBoxItem ToVisual()
        {
            return new ListBoxItem()
            {
                Content = new TextBlock()
                {
                    Text = string.Format("#{0} - {1}, {2} points",
                                         Score.Rank, Score.Player, Score.Score)
                }
            };
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
}