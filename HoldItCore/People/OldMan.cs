using System.Collections.Generic;

namespace HoldItCore.People {
	public class OldMan: Person {

		private Randomizer waitingMessages = new Randomizer();
		private Randomizer headingToStallMessages = new Randomizer();
		private Randomizer enteredStallMessages = new Randomizer();
		private Randomizer bladderEmptyMessages = new Randomizer();

		public OldMan() {
			this.DefaultStyleKey = typeof(OldMan);

			this.WalkSpeed = 200;

			this.waitingMessages.Add(.1, delegate {
				this.Say("Too many youngins.");
			});

			this.waitingMessages.Add(.1, delegate {
				this.Say("Whippersnappers.");
			});

			this.waitingMessages.Add(.1, delegate {
				this.Say("Get off my lawn.");
			});

			this.headingToStallMessages.Add(.3, delegate {
				this.Say("Just a minute.");
			});
			this.headingToStallMessages.Add(.3, delegate {
				this.Say("One second sonny.");
			});
			this.headingToStallMessages.Add(.3, delegate {
				this.Say("Hold yer horses.");
			});

			this.enteredStallMessages.Add(.5, delegate {
				this.Say("Give me a minute.");
			});

			this.bladderEmptyMessages.Add(.3, delegate {
				this.Say("Hotdog!");
			});

			this.bladderEmptyMessages.Add(.3, delegate {
				this.Say("Thanks sonny.");
			});
		}

		protected override void OnWait() {
			base.OnWait();

			this.waitingMessages.DoSomething();
		}

		protected override void OnHeadingToStall() {
			base.OnHeadingToStall();

			this.headingToStallMessages.DoSomething();
		}

		protected override void OnEnteredStall() {
			base.OnEnteredStall();

			this.enteredStallMessages.DoSomething();
		}

		protected override void OnBladderEmpty() {
			base.OnBladderEmpty();

			this.bladderEmptyMessages.DoSomething();
		}

		protected override void OnPeedPants() {
			base.OnPeedPants();

			this.Say("Not again!");
		}
	}
}
