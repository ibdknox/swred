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
				Description = "The exposed brick is soooo hip",
			});

			LevelInfo.AllLevels.Add(new LevelInfo() {
				LevelType = typeof(InteriorDesign),
				Title = "Super Bowls",
				Description = "Welcome to the big leagues",
			});

			LevelInfo.AllLevels.Add(new LevelInfo() {
				LevelType = typeof(Urination),
				Title = "Urine Nation",
				Description = "Everyone on earth goes to the John, right now",
			});
		}

		public Type LevelType { get; set; }

		public string Title { get; set; }
		public bool Completed { get; set; }

		public string Description { get; set; }
	}
}
