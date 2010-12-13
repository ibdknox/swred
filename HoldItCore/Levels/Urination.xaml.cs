

using System.Windows.Threading;
using System;
using HoldItCore.People;
using System.Windows.Controls;

namespace HoldItCore.Levels {
	public partial class Urination : Level {

		DispatcherTimer timer = new DispatcherTimer() {
			Interval = TimeSpan.FromSeconds(3),
		};

		private Random rng = new Random();

		public Urination() {
			InitializeComponent();

			this.Remaining = 25;

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
			if (val <= .2)
				this.AddPerson(new Nerves());
			else if (val < .5)
				this.AddPerson(new OldMan());
			else
				this.AddPerson(new Normal());
		}
	}
}
