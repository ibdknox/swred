

using System.Collections.Generic;

namespace HoldItCore {
	public class Randomizer {
		public delegate void Action();

		private struct Entry {
			public double Chance;
			public Action Action;
		}

		private List<Entry> entries = new List<Entry>();

		public void Add(double chance, Action action) {
			this.entries.Add(new Entry() {
				Action = action,
				Chance = chance,
			});
		}

		public void DoSomething() {
			double val = Utils.RNG.NextDouble();
			double bar = 0;
			foreach (Entry entry in this.entries) {
				bar += entry.Chance;

				if (val < bar) {
					entry.Action();
					return;
				}
			}
		}
	}
}
