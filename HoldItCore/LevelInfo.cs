using System;
using System.Collections.Generic;
using HoldItCore.Levels;
namespace HoldItCore {

	public class LevelInfo {

		public static List<LevelInfo> AllLevels = new List<LevelInfo>();

		static LevelInfo() {
			
			LevelInfo.AllLevels.Add(new LevelInfo() {
				LevelType = typeof(Easy),
				Title = "Potty Training",
				Description = "Everyone has to start somewhere",
			});

			LevelInfo.AllLevels.Add(new LevelInfo() {
				LevelType = typeof(InteriorDesign),
				Title = "Interior Design",
				Description = "Classy!",
			});

			LevelInfo.AllLevels.Add(new LevelInfo() {
				LevelType = typeof(IntroLevel),
				Title = "Super Bowls",
				Description = "Welcome to the big leagues",
			});

			LevelInfo.AllLevels.Add(new LevelInfo() {
				LevelType = typeof(IntroLevel),
				Title = "Urine Nation",
				Description = "",
			});
		}

		public Type LevelType { get; set; }

		public string Title { get; set; }
		public bool Completed { get; set; }

		public string Description { get; set; }
	}
}
