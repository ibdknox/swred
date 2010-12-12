

using System.Windows.Threading;
using System;
using HoldItCore.People;

namespace HoldItCore.Levels {
	public partial class InteriorDesign : Level {

		DispatcherTimer timer = new DispatcherTimer() {
			Interval = TimeSpan.FromSeconds(3),
		};

		private Random rng = new Random();

		public InteriorDesign() {
			InitializeComponent();

			this.timer.Tick += this.HandleTick;
		}

		protected override void Start() {
			base.Start();

			this.timer.Start();

			this.Spawn();
		}

		protected override void Stop() {
			base.Stop();

			this.timer.Stop();
		}

		private void HandleTick(object sender, EventArgs e) {
			this.Spawn();
		}

		private void Spawn() {
			double val = rng.NextDouble();
			if (val <= .5)
				this.AddPerson(new Nerves());
			else
				this.AddPerson(new OldMan());
		}
	}
}
