using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows;
using System.Diagnostics;
using System.ComponentModel;

namespace HoldItCore {

	public enum LevelState {
		Intro,
		Playing,
		Completed,
		Failed,
	}

	public class Level : ContentControl {

		private List<Stall> stalls = new List<Stall>();

		private List<Person> waitingLine = new List<Person>();

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

			if (!DesignerProperties.IsInDesignTool)
				this.State = LevelState.Intro;

			VisualStateGroup playStates = (VisualStateGroup)this.GetTemplateChild("PlayStates");
			playStates.CurrentStateChanged += this.HandleCurrentStateChanged;
		}

		private void HandleCurrentStateChanged(object sender, VisualStateChangedEventArgs e) {
			if (e.NewState.Name == LevelState.Intro.ToString()) {
				this.State = LevelState.Playing;
				this.Start();
			}
		}

		protected void AddPerson(Person person) {
			person.Level = this;
			this.peopleCanvas.Children.Add(person);

			this.waitingLine.Add(person);
			

			this.UpdateWaitingLine();
		}

		protected void Completed() {
		}

		public void RemoveFromLine(Person person) {
			Debug.Assert(this.waitingLine.Contains(person));

			this.waitingLine.Remove(person);
		}

		public void RemovePerson(Person person) {
			this.OnPersonRemoved(person);
			this.peopleCanvas.Children.Remove(person);
			
		}

		protected virtual void OnPersonRemoved(Person person) {
		}

		private void UpdateWaitingLine() {
			int index = 0;
			foreach (Person person in this.waitingLine) {
				person.PositionWaitingLine(index);
				++index;
			}
		}

		public void SendPersonTo(Stall stall) {
			if (this.selection != null) {

				this.selection.GoToStall(stall);
				this.Deselect(this.selection);

				this.UpdateWaitingLine();
			}
		}

		protected virtual void Start() {
		}

		protected virtual void Stop() {
		}

		private Person selection = null;
		public void Select(Person person) {
			if (this.selection != null)
				this.selection.IsSelected = false;

			this.selection = person;
			if (this.selection != null)
				this.selection.IsSelected = true;
		}

		public void Deselect(Person person) {
			person.IsSelected = false;
			if (this.selection == person)
				this.selection = null;
		}

		private LevelState state;
		public LevelState State {
			get { return this.state; }
			set {
				this.state = value;
				VisualStateManager.GoToState(this, this.State.ToString(), true);
			}
		}
	}
}
