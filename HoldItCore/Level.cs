using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows;
using System.Diagnostics;
using System.ComponentModel;
using System;
using System.Linq;
using HoldItCore.People;
using System.Net;

namespace HoldItCore {

	public enum LevelState {
		Intro,
		Playing,
		Completed,
		Failed,
	}

	public class Level : ContentControl {

		public static List<string> LevelNames = new List<string>()
		{
			"Easy",
			"Intro"
		};

		private List<Stall> stalls = new List<Stall>();

		private List<Person> waitingLine = new List<Person>();

		private Panel peopleCanvas;

		public int CurrentPeopleInFlight { get; private set; }
		

		public Level() {
			this.DefaultStyleKey = typeof(Level);
			this.DataContext = this;
		}

		public event EventHandler Completed;

		public override void OnApplyTemplate() {
			base.OnApplyTemplate();

			FrameworkElement content = (FrameworkElement)this.Content;
			Panel layoutRoot = (Panel)content.FindName("Urinals");
			if (layoutRoot != null) {

				foreach (FrameworkElement child in layoutRoot.Children) {
					Stall stall = child as Stall;
					if (stall != null) {
						stalls.Add(stall);
						stall.Level = this;
					}
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
			if (this.Remaining == 0)
				return;

			person.Level = this;

			this.CurrentPeopleInFlight++;
			this.Remaining--;

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
			this.UpdateWaitingLine();
		}

		public void RemovePerson(Person person) {
			this.OnPersonRemoved(person);
			this.peopleCanvas.Children.Remove(person);

			this.CurrentPeopleInFlight--;

			if (this.CurrentPeopleInFlight == 0 || this.Remaining == 0)
			{
				// Level is finished!
				this.OnCompleted();
			}
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

				if (this.selection.CanUseStall(stall)) {

					this.selection.GoToStall(stall);
					this.Deselect(this.selection);

					this.UpdateWaitingLine();
				}
			}
		}

		public void ScoreStallChoice(Stall stall) {
			int neighbors = this.GetNeighborCount(stall);
			if (neighbors > 0)
			{
				this.AdjustScore(neighbors * (-15));
				stall.Alert(neighbors * (-15), "Too close!");
			}
		}

		public int GetNeighborCount(Stall stall) {
			int stallIndex = this.stalls.IndexOf(stall);
			int neighbors = 0;
			if (stallIndex > 0 && this.stalls[stallIndex - 1].Person != null)
				++neighbors;
			if (stallIndex < this.stalls.Count - 1 && this.stalls[stallIndex + 1].Person != null)
				++neighbors;

			return neighbors;
		}

		public IEnumerable<Stall> GetNeighbors(Stall stall) {
			int stallIndex = this.stalls.IndexOf(stall);
			if (stallIndex > 0)
				yield return this.stalls[stallIndex - 1];
			if (stallIndex < this.stalls.Count - 1)
				yield return this.stalls[stallIndex + 1];
		}

		protected virtual void Start() {
		}

		public virtual void Stop() {
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

		public void AdjustScore(int change)
		{
			Score += change;
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

		public static readonly DependencyProperty RemainingProperty = DependencyProperty.Register("Remaining", typeof(int), typeof(Level), new PropertyMetadata(default(int)));
		public int Remaining
		{
			get { return (int)this.GetValue(Level.RemainingProperty); }
			set { this.SetValue(Level.RemainingProperty, value); }
		}

		private LevelState state;
		public LevelState State {
			get { return this.state; }
			set {
				this.state = value;
				VisualStateManager.GoToState(this, this.State.ToString(), true);
			}
		}

		public static readonly DependencyProperty ScoreIncrementProperty = DependencyProperty.Register("ScoreIncrement", typeof(double), typeof(Level), new PropertyMetadata(default(double)));
		public double ScoreIncrement {
			get { return (double)this.GetValue(Level.ScoreIncrementProperty); }
			set { this.SetValue(Level.ScoreIncrementProperty, value); }
		}

		
		public static readonly DependencyProperty ScoreReasonProperty = DependencyProperty.Register("ScoreReason", typeof(string), typeof(Level), new PropertyMetadata(default(string)));
		public string ScoreReason {
			get { return (string)this.GetValue(Level.ScoreReasonProperty); }
			set { this.SetValue(Level.ScoreReasonProperty, value); }
		}

		
	}
}
