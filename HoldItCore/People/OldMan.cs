using System.Collections.Generic;

namespace HoldItCore.People {
	public class OldMan: Person {

		
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
				this.Say("Get off my lawn!");
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

			this.enteredStallMessages.Add(.3, delegate {
				this.Say("Give me a minute.");
			});

			this.enteredStallMessages.Add(.3, delegate {
				this.Say("Better than the diapers!");
			});



			this.bladderEmptyMessages.Add(.3, delegate {
				this.Say("Hot dog!");
			});

			this.bladderEmptyMessages.Add(.3, delegate {
				this.Say("Thanks sonny.");
			});

			this.peedMessages.Add(1, delegate {
				this.Say("Not again!");
			});
		}

		
	}
}
