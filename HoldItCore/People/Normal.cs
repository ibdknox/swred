
namespace HoldItCore.People {
	public class Normal: Person {


		public Normal() {
			this.waitingMessages.Add(.1, delegate {
				this.Say("Hurry up.");
			});

			this.waitingMessages.Add(.1, delegate {
				this.Say("It's crazy in here.");
			});

			this.headingToStallMessages.Add(.3, delegate {
				this.Say("'Scuze me.");
			});


			this.enteredStallMessages.Add(.3, delegate {
				this.Say("Finally.");
			});
			this.enteredStallMessages.Add(.3, delegate {
				this.Say("That was close.");
			});



			this.bladderEmptyMessages.Add(.5, delegate {
				this.Say("Thanks.");
			});


			this.peedMessages.Add(.4, delegate {
				this.Say("Crap!");
			});

			this.peedMessages.Add(.4, delegate {
				this.Say("Oh well.");
			});

			this.peedMessages.Add(.2, delegate {
				this.Say("It was the sink.");
			});
		}

		protected override void OnPeedPants() {
			base.OnPeedPants();

			this.Say("Not again!");
		}

	}
}
