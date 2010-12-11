using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows;

namespace HoldItCore {
	public class Level : ContentControl {

		private List<Stall> stalls = new List<Stall>();

		private List<Person> persons = new List<Person>();
		private Queue<Person> waitingLine = new Queue<Person>();

		private Panel peopleCanvas;

		public Level() {
			this.DefaultStyleKey = typeof(Level);
		}

		public override void OnApplyTemplate() {
			base.OnApplyTemplate();

			FrameworkElement content = (FrameworkElement)this.Content;
			Panel layoutRoot = (Panel)content.FindName("Urinals");

			foreach (FrameworkElement child in layoutRoot.Children) {
				Stall stall = child as Stall;
				if (stall != null) {
					stalls.Add(stall);
					stall.Level = this;
				}
			}

			this.peopleCanvas = (Panel)content.FindName("People");

			//FrameworkElement lineStartElement = content.FindName("LineStart") as FrameworkElement;

			//Point lineStart = lineStartElement.TransformToVisual(this.peopleCanvas).Transform(new Point(0,0));

			this.Start();
		}

		protected void AddPerson(Person person) {
			person.Level = this;
			this.peopleCanvas.Children.Add(person);

			//this.persons.Add(person);
			this.waitingLine.Enqueue(person);
			

			this.UpdateWaitingLine();
		}

		public void RemovePerson(Person person) {
			this.peopleCanvas.Children.Remove(person);
		}

		private void UpdateWaitingLine() {
			int index = 0;
			foreach (Person person in this.waitingLine) {
				person.PositionWaitingLine(index);
				++index;
			}
		}

		public void SendPersonTo(Stall stall) {
			if (this.waitingLine.Count > 0) {
				Person first = this.waitingLine.Dequeue();
				first.GoToStall(stall);

				this.UpdateWaitingLine();
			}
		}

		protected virtual void Start() {
		}

		protected virtual void Stop() {
		}
	}
}
