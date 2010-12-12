namespace HoldItCore.People {
	public class Nerves: Person {

		public Nerves() {
			this.DefaultStyleKey = typeof(Nerves);
			this.BladderFillRate = .01 + Utils.RNG.NextDouble() * .01;
		}

		public override bool CanUseStall(Stall stall) {
			bool canUse = stall.OccupiedNeighborCount == 0;

			if (!canUse) {
				this.SpeechText = "";
				this.SpeechText = "No way! Germs!";
			}

			return canUse;
		}

		protected override void OnNeighborEnteredStall() {
			base.OnNeighborEnteredStall();

			this.SpeechText = "";
			this.SpeechText = "Get away. I'm busy here.";

			this.StopPeeing();
		}

		protected override void OnNeighborLeftStall() {
			this.StartPeeing();
		}
	}
}
