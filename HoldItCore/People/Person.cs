

using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System;
using System.Diagnostics;
using System.ComponentModel;
using HoldItCore.Sounds;

namespace HoldItCore.People {

	public enum PersonState {
		Unset,
		InLine,
		HeadedToStall,
		InStall,
		PeedPants,
		Exiting,
	}

	public class Person : Control {

		private Point lineStart = new Point(35, 400);
		private Point exitPoint = new Point(-100, 500);
		private Point enterPoint = new Point(900, 400);
		private Stall stall;
		private TranslateTransform translation = new TranslateTransform();
		private ScaleTransform peeScaleTransform = new ScaleTransform();
		private Storyboard bladderFillAnimation = new Storyboard();
		private Storyboard bladderEmptyAnimation = new Storyboard();

		public Person() {
			this.DefaultStyleKey = typeof(Person);

			this.RenderTransform = translation;

			this.InitialBladderFill = .2 + Utils.RNG.NextDouble() * .2;
			this.BladderFillRate = .1 + Utils.RNG.NextDouble() * .1;
			this.PeeRate = .05 + Utils.RNG.NextDouble() * .1;
			this.State = PersonState.Unset;

			this.WalkSpeed = 400;
		}

		public override void OnApplyTemplate() {
			base.OnApplyTemplate();

			//this.peeScaleTransform = (ScaleTransform)this.GetTemplateChild("PeeScaleTransform");
			((FrameworkElement)this.GetTemplateChild("PeeProgress")).RenderTransform = this.peeScaleTransform;

			// Stop the animations at design-time.
			if (!DesignerProperties.IsInDesignTool)
				this.StartBladderFilling();

			VisualStateGroup textStates = (VisualStateGroup)this.GetTemplateChild("TextStates");
			textStates.CurrentStateChanged += this.HandleTextStatesChanged;
		}

		public Level Level { get; set; }

		/// <summary>
		/// Percent that the bladder is full when the person starts
		/// </summary>
		public double InitialBladderFill { get; set; }

		public double CurrentBladderFill
		{
			get { return this.peeScaleTransform.ScaleX; }
		}

		/// <summary>
		/// Rate that the person's bladder fills, in percent per second
		/// </summary>
		public double BladderFillRate { get; set; }

		/// <summary>
		/// Rate that the person pees, in percent per second.
		/// </summary>
		public double PeeRate { get; set; }


		/// <summary>
		/// Speed that the person walks.
		/// </summary>
		public double WalkSpeed { get; set; }

		/// <summary>
		/// For debugging.
		/// </summary>
		public PersonState State { get; set; }

		public void PositionWaitingLine(int index) {
			Point position = new Point(lineStart.X + 50 * index, lineStart.Y);

			if (this.State != PersonState.Unset)
				this.AnimateTo(position);
			else {
				this.MoveTo(this.enterPoint);
				this.AnimateTo(position, this.WalkSpeed);

				this.State = PersonState.InLine;
			}

			
		}

		public void GoToStall(Stall stall) {
			this.State = PersonState.HeadedToStall;

			stall.PersonEntering(this);
			this.stall = stall;

			Point stallPosition = stall.TransformToVisual((FrameworkElement)this.Parent).Transform(new Point(0, 0));
			Storyboard sb = this.AnimateTo(stallPosition);

			this.Level.RemoveFromLine(this);

			sb.Completed += this.HandleGoToStallAnimationCompleted;
		}

		private void HandleGoToStallAnimationCompleted(object sender, EventArgs e) {
			// May have had an accident on way to stall.
			if (this.stall != null) {
				this.OnEnteredStall();
			}
		}

		public void StartBladderFilling() {
			this.peeScaleTransform.ScaleX = this.InitialBladderFill;
			this.bladderFillAnimation = this.AnimatePeeTo(1, this.BladderFillRate);

			this.bladderFillAnimation.Completed += this.HandleBladderFillAnimationCompleted;
		}

		private void HandleBladderFillAnimationCompleted(object sender, EventArgs e) {

			VisualStateManager.GoToState(this, "PeedPants", true);

			this.Level.AccidentHappened();

			if (this.stall != null) {
				this.stall.PersonLeft();
				this.stall = null;
			}
			else {
				if (this.Level != null)
					this.Level.RemoveFromLine(this);
			}

			if (this.IsSelected)
				this.Level.Deselect(this);

			Storyboard exitingAnimation = this.AnimateTo(this.exitPoint);
			exitingAnimation.Completed += this.HandleExitCompleted;
		}

		private double MaxPeeAmount { get; set; }

		private void StartPeeing() {
            SoundManager.Play(SoundIndex.peeing, false);
			this.MaxPeeAmount = this.CurrentBladderFill;
			double scale = this.CurrentBladderFill;
			this.bladderFillAnimation.Stop();
			this.peeScaleTransform.ScaleX = scale;
			this.bladderEmptyAnimation = this.AnimatePeeTo(0, this.PeeRate);
			this.bladderEmptyAnimation.Completed += this.HandleBladderEmptyAnimationCompleted;
		}

		private void HandleBladderEmptyAnimationCompleted(object sender, EventArgs e) {
			if (this.stall != null)
				this.LeaveStall();
		}

		private Storyboard AnimatePeeTo(double percent, double rate) {
			double startScale = this.peeScaleTransform.ScaleX;

			DoubleAnimation animation = new DoubleAnimation() {
				To = percent,
				From = startScale,
				Duration = TimeSpan.FromSeconds(Math.Abs(percent - startScale) / rate),
			};
			Storyboard.SetTargetProperty(animation, new PropertyPath(ScaleTransform.ScaleXProperty));

			Storyboard sb = new Storyboard();
			Storyboard.SetTarget(sb, this.peeScaleTransform);
			sb.Children.Add(animation);

			sb.Begin();

			return sb;
		}
		

		protected virtual void OnEnteredStall() {
			this.State = PersonState.InStall;

			this.Level.ScoreStallChoice(this.stall);

			this.SpeechText = "Aaahhh!!!";

			this.stall.PersonEntered();
			this.StartPeeing();
		}

		protected virtual void LeaveStall() {
			this.State = PersonState.Exiting;
			this.Level.ModifyScore((int)(this.MaxPeeAmount * 100), "Finished!");
			Storyboard sb = this.AnimateTo(this.exitPoint);
			this.stall.PersonLeft();
			sb.Completed += this.HandleExitCompleted;
		}

		private void HandleExitCompleted(object sender, EventArgs e) {
			if (this.Level != null)
				this.Level.RemovePerson(this);
		}

		private void MoveTo(Point point) {
			this.translation.X = point.X;
			this.translation.Y = point.Y;
		}

		private Storyboard AnimateTo(Point point) {
			return this.AnimateTo(point, this.WalkSpeed);
		}

		private Storyboard AnimateTo(Point point, double rate) {

			double xOffset = point.X - this.translation.X;
			double yOffset = point.Y - this.translation.Y;

			double distance = Math.Sqrt(xOffset * xOffset + yOffset * yOffset);

			
			Duration duration = new Duration(TimeSpan.FromSeconds(distance / rate));
			DoubleAnimation xAnim = new DoubleAnimation() {
				To = point.X,
				Duration = duration,
			};
			Storyboard.SetTargetProperty(xAnim, new PropertyPath(TranslateTransform.XProperty));

			DoubleAnimation yAnim = new DoubleAnimation() {
				To = point.Y,
				Duration = duration,
			};
			Storyboard.SetTargetProperty(yAnim, new PropertyPath(TranslateTransform.YProperty));

			Storyboard sb = new Storyboard();
			sb.Children.Add(xAnim);
			sb.Children.Add(yAnim);

			Storyboard.SetTarget(sb, this.translation);

			sb.Begin();

			return sb;
		}

		protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e) {
			base.OnMouseLeftButtonDown(e);

			this.Level.Select(this);
		}

		public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(Person), new PropertyMetadata(default(bool), Person.HandleIsSelectedChanged));
		public bool IsSelected {
			get { return (bool)this.GetValue(Person.IsSelectedProperty); }
			set { this.SetValue(Person.IsSelectedProperty, value); }
		}

		private static void HandleIsSelectedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
			((Person)sender).OnIsSelectedChanged(e);
		}

		protected virtual void OnIsSelectedChanged(DependencyPropertyChangedEventArgs e) {
			if (this.IsSelected)
				VisualStateManager.GoToState(this, "Selected", true);
			else
				VisualStateManager.GoToState(this, "Deselected", true);
		}



		public static readonly DependencyProperty SpeechTextProperty = DependencyProperty.Register("SpeechText", typeof(string), typeof(Person), new PropertyMetadata(default(string), Person.HandleSpeechTextChanged));
		public string SpeechText {
			get { return (string)this.GetValue(Person.SpeechTextProperty); }
			set { this.SetValue(Person.SpeechTextProperty, value); }
		}

		private static void HandleSpeechTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
			((Person)sender).OnSpeechTextChanged(e);
		}

		protected virtual void OnSpeechTextChanged(DependencyPropertyChangedEventArgs e) {
			VisualStateManager.GoToState(this, "Visible", true);
		}

		private void HandleTextStatesChanged(object sender, VisualStateChangedEventArgs e) {
			if (e.NewState.Name == "Visible")
				VisualStateManager.GoToState(this, "Hidden", true);
		}
	}
}
