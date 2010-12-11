using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Diagnostics;

namespace HoldItCore.Levels {
	public partial class Easy : Level {

		DispatcherTimer timer = new DispatcherTimer() {
			Interval = TimeSpan.FromSeconds(1.5),
		};

		private int toBeSpawnedCount = 3;
		private int inFlightCount = 0;

		public Easy() {
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
			if (this.toBeSpawnedCount > 0) {
				this.AddPerson(new Person());
				++this.inFlightCount;
				--this.toBeSpawnedCount;
			}
			else {
				this.timer.Stop();
			}
		}

		protected override void OnPersonRemoved(Person person) {
			base.OnPersonRemoved(person);

			--this.inFlightCount;
			Debug.Assert(this.inFlightCount >= 0);

			if (this.toBeSpawnedCount == 0 && this.inFlightCount == 0) {
				this.OnCompleted();
			}
		}




	}
}
