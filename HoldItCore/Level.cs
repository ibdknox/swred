using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows;
using System.Diagnostics;
using System.ComponentModel;
using System;

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
			this.DataContext = this;
		}

		public event EventHandler Completed;

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
			else if (e.NewState.Name == LevelState.Completed.ToString()) {
				if (this.Completed != null)
					this.Completed(this, EventArgs.Empty);
			}
		}

		protected void AddPerson(Person person) {
			person.Level = this;
			this.peopleCanvas.Children.Add(person);

			this.waitingLine.Add(person);
			

			this.UpdateWaitingLine();
		}

		protected virtual void OnCompleted() {
			this.State = LevelState.Completed;
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

				int stallIndex = this.stalls.IndexOf(stall);

				if (stallIndex > 0 && this.stalls[stallIndex - 1].Person != null)
					this.ModifyScore(-15, "Too close!");
				if (stallIndex < this.stalls.Count - 1 && this.stalls[stallIndex + 1].Person != null)
					this.ModifyScore(-15, "Too close!");

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

		/// <summary>
		/// Modify the score with the given value (may be negative, for negative points)
		/// </summary>
		public void ModifyScore(int incrementValue, string reason)
		{
			Score = Score + incrementValue;

			ScoreModification modification = new ScoreModification()
			{
				ModificationType = incrementValue > 0 ? ModificationType.Positive : ModificationType.Negative,
				AbsoluteValue = Math.Abs(incrementValue),
				Reason = reason
			};

			LastScoreModification = modification;
		}

		public void AccidentHappened()
		{
			this.Accidents += 1;
		}

		public static readonly DependencyProperty ScoreProperty = DependencyProperty.Register("Score", typeof(int), typeof(Level), new PropertyMetadata(default(int)));
		private int Score
		{
			get { return (int)this.GetValue(Level.ScoreProperty); }
			set { this.SetValue(Level.ScoreProperty, value); }
		}

		public static readonly DependencyProperty AccidentsProperty = DependencyProperty.Register("Accidents", typeof(int), typeof(Level), new PropertyMetadata(default(int)));
		private int Accidents
		{
			get { return (int)this.GetValue(Level.AccidentsProperty); }
			set { this.SetValue(Level.AccidentsProperty, value); }
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
