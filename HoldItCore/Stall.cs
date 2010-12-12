using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;
using HoldItCore.People;

namespace HoldItCore {

	public class Stall : Control {
		public Stall() {
			this.DefaultStyleKey = typeof(Stall);
		}

		public Level Level { get; set; }
		
		private Person person;
		public Person Person {
			get { return this.person; }
			private set {
				Debug.Assert((this.person == null && value != null) || (this.person != null && value == null));
				this.person = value;
			}
		}

		public void PersonEntering(Person person) {
			this.Person = person;
		}

		public void PersonEntered() {
		}

		public void PersonLeft() {
			this.Person = null;
		}

		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {
			base.OnMouseLeftButtonDown(e);

			if (this.Person == null)
				this.Level.SendPersonTo(this);
		}
	}
}
