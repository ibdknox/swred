using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;
using HoldItCore.People;
using System.Collections.Generic;

namespace HoldItCore {

	public class Stall : Control {
		public Stall() {
			this.DefaultStyleKey = typeof(Stall);
		}

		public Level Level { get; set; }

		private Panel alertPanel; 

		private Person person;
		public Person Person {
			get { return this.person; }
			private set {
				Debug.Assert((this.person == null && value != null) || (this.person != null && value == null));
				this.person = value;
			}
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.alertPanel = (Panel)this.GetTemplateChild("AlertPanel");
		}

		public void Alert(int incrementValue, string reason)
		{
			ScoreAlert alert = new ScoreAlert();
			this.alertPanel.Children.Add(alert);

			alert.Alert = incrementValue;
			alert.Description = reason;
		}

		public void PersonEntering(Person person) {
			this.Person = person;
		}

		public void PersonEntered() {
		}

		public void PersonLeft() {
			this.Person = null;
		}

		public int OccupiedNeighborCount {
			get { return this.Level.GetNeighborCount(this); }
		}

		public IEnumerable<Stall> Neighbors {
			get { return this.Level.GetNeighbors(this); }
		}

		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {
			base.OnMouseLeftButtonDown(e);

			if (this.Person == null)
				this.Level.SendPersonTo(this);
		}
	}
}
