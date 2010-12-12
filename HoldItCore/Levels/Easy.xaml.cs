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
using HoldItCore.People;

namespace HoldItCore.Levels {
	public partial class Easy : Level {

		DispatcherTimer timer = new DispatcherTimer() {
			Interval = TimeSpan.FromSeconds(1.5),
		};

		public Easy() {
			InitializeComponent();

            this.Remaining = 5;

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
		    this.AddPerson(new Person());
		}
	}
}
