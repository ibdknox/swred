using System;
using System.Collections.Generic;
using HoldItCore.Levels;
namespace HoldItCore {

	public class LevelInfo {

		public static List<LevelInfo> AllLevels = new List<LevelInfo>();

		static LevelInfo() {
			LevelInfo.AllLevels.Add(new LevelInfo() {
				LevelType = typeof(InteriorDesign),
				Title = "Interior Design",
				Description = "Classy!",
			});

			LevelInfo.AllLevels.Add(new LevelInfo() {
				LevelType = typeof(Easy),
				Title = "Easy",
				Description = "Just getting started.",
			});

			LevelInfo.AllLevels.Add(new LevelInfo() {
				LevelType = typeof(IntroLevel),
				Title = "Intro",
				Description = "Never going to get out of here!",
			});
		}

		public Type LevelType { get; set; }

		public string Title { get; set; }
		public bool Completed { get; set; }

		public string Description { get; set; }
	}
}
