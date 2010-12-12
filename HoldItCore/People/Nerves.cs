namespace HoldItCore.People {
	public class Nerves: Person {

		private Randomizer cantUseStallMessages = new Randomizer();
		private Randomizer neighborAddedMessages = new Randomizer();

		public Nerves() {
			this.DefaultStyleKey = typeof(Nerves);
			this.BladderFillRate = .01 + Utils.RNG.NextDouble() * .01;

			// Waiting
			this.waitingMessages.Add(.1, delegate {
				this.Say("Oh God Oh God Oh God");
			});

			this.waitingMessages.Add(.1, delegate {
				this.Say("EEEEeeeeee!");
			});

			// Headed to stall.
			this.headingToStallMessages.Add(.1, delegate {
				this.Say("Oh God Oh God Oh God");
			});

			this.headingToStallMessages.Add(.1, delegate {
				this.Say("EEEEeeeeee!");
			});

			// Entered stall
			this.enteredStallMessages.Add(.1, delegate {
				this.Say("Oh God Oh God Oh God");
			});

			this.enteredStallMessages.Add(.1, delegate {
				this.Say("EEEEeeeeee!");
			});

			// After
			this.bladderEmptyMessages.Add(.1, delegate {
				this.Say("Don't touch me!!!");
			});

			this.bladderEmptyMessages.Add(.1, delegate {
				this.Say("Get me outta here!");
			});

			// Peed pants
			this.peedMessages.Add(1, delegate {
				this.Say("Crap crap crap!");
			});


			// Can't use the stall
			this.cantUseStallMessages.Add(.5, delegate {
				this.Say("No way! Germs!");
			});

			this.cantUseStallMessages.Add(.5, delegate {
				this.Say("Eeeew!");
			});

			// Neighbor added
			this.neighborAddedMessages.Add(.3, delegate {
				this.Say("Get away!");
			});

			this.neighborAddedMessages.Add(.3, delegate {
				this.Say("Shoo!");
			});

			this.neighborAddedMessages.Add(.3, delegate {
				this.Say("I'll just wait.");
			});

		}

		public override bool CanUseStall(Stall stall) {
			bool canUse = stall.OccupiedNeighborCount == 0;

			if (!canUse)
				this.cantUseStallMessages.DoSomething();

			return canUse;
		}

		protected override void OnNeighborEnteredStall() {
			base.OnNeighborEnteredStall();

			this.neighborAddedMessages.DoSomething();

			this.StopPeeing();
		}

		protected override void OnNeighborLeftStall() {
			this.StartPeeing();
		}
	}
}
