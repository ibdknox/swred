

using System.Windows.Threading;
using System;
using HoldItCore.People;

namespace HoldItCore.Levels {
	public partial class IntroLevel : Level {

		DispatcherTimer timer = new DispatcherTimer() {
			Interval = TimeSpan.FromSeconds(3),
		};

		private Random rng = new Random();

		public IntroLevel() {
			InitializeComponent();

            this.Remaining = 25;

			this.timer.Tick += this.HandleTick;
		}

		protected override void Start() {
			base.Start();

			this.timer.Start();

			this.AddPerson(new Person());
		}

		public override void Stop() {
			base.Stop();

			this.timer.Stop();
		}

		private void HandleTick(object sender, EventArgs e) {
			double val = rng.NextDouble();
			if (val <= .5)
				this.AddPerson(new OldMan());
			else
				this.AddPerson(new Person());
		}
	}
}
