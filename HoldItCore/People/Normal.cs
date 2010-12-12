
namespace HoldItCore.People {
	public class Normal: Person {

		private Randomizer waitingMessage = new Randomizer();
		private Randomizer headingToStallMessages = new Randomizer();
		private Randomizer enteredStallMessages = new Randomizer();
		private Randomizer bladderEmptyMessages = new Randomizer();

		public Normal() {
		}

		protected override void OnWait() {
			base.OnWait();
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
