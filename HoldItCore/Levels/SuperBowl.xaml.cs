

using System.Windows.Threading;
using System;
using HoldItCore.People;
using System.Windows.Controls;

namespace HoldItCore.Levels {
	public partial class SuperBowl : Level {

		DispatcherTimer timer = new DispatcherTimer() {
			Interval = TimeSpan.FromSeconds(1),
		};

		private Random rng = new Random();

		public SuperBowl() {
			InitializeComponent();

			this.Remaining = 50;

			this.timer.Tick += this.HandleTick;
		}

		protected override void Start() {
			base.Start();

			this.timer.Start();

			this.Spawn();
		}

		public override void Stop() {
			base.Stop();

			this.timer.Stop();
		}

		private void HandleTick(object sender, EventArgs e) {
			this.Spawn();
		}

		private void Spawn() {
			double val = rng.NextDouble();
			Person person;
			if (val <= .05)
				person = new Nerves();
			else if (val < .3)
				person = new OldMan();
			else
				person = new Normal();

			person.WalkSpeed = person.WalkSpeed * 2;
			person.PeeRate = person.PeeRate * 2;

			this.AddPerson(person);
		}
	}
}
