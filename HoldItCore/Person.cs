

using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System;

namespace HoldItCore {
	public class Person : Control {

		private Point lineStart = new Point(35, 400);
		private bool positioned = false;
		private Stall stall;
		private TranslateTransform translation = new TranslateTransform();
		private ScaleTransform peeScaleTransform = new ScaleTransform();
		private Storyboard bladderFillAnimation = new Storyboard();
		private Storyboard bladderEmptyAnimation = new Storyboard();

		public Person() {
			this.DefaultStyleKey = typeof(Person);

			this.RenderTransform = translation;

			this.InitialBladderFill = .5;
			this.BladderFillRate = .1;
			this.PeeRate = .2;
		}

		public override void OnApplyTemplate() {
			base.OnApplyTemplate();

			//this.peeScaleTransform = (ScaleTransform)this.GetTemplateChild("PeeScaleTransform");
			((FrameworkElement)this.GetTemplateChild("PeeProgress")).RenderTransform = this.peeScaleTransform;
			this.StartBladderFilling();
		}

		public Level Level { get; set; }

		/// <summary>
		/// Percent that the bladder is full when the person starts
		/// </summary>
		public double InitialBladderFill { get; set; }

		/// <summary>
		/// Rate that the person's bladder fills, in percent per second
		/// </summary>
		public double BladderFillRate { get; set; }

		/// <summary>
		/// Rate that the person pees, in percent per second.
		/// </summary>
		public double PeeRate { get; set; }

		public void PositionWaitingLine(int index) {
			Point position = new Point(lineStart.X + 50 * index, lineStart.Y);

			if (this.positioned)
				this.AnimateTo(position);
			else {
				this.MoveTo(position);
				this.positioned = true;
			}
		}

		public void GoToStall(Stall stall) {
			stall.PersonEntering(this);
			this.stall = stall;

			Point stallPosition = stall.TransformToVisual((FrameworkElement)this.Parent).Transform(new Point(0, 0));
			Storyboard sb = this.AnimateTo(stallPosition);

			sb.Completed += this.HandleGoToStallAnimationCompleted;
		}

		private void HandleGoToStallAnimationCompleted(object sender, EventArgs e) {
			this.OnEnteredStall();
		}

		public void StartBladderFilling() {
			DoubleAnimation bladderFillScale = new DoubleAnimation() {
				From = this.InitialBladderFill,
				To = 1,
				Duration = TimeSpan.FromSeconds((1 - this.InitialBladderFill) / this.BladderFillRate),
			};
			Storyboard.SetTargetProperty(bladderFillScale, new PropertyPath(ScaleTransform.ScaleXProperty));

			
			Storyboard.SetTarget(this.bladderFillAnimation, this.peeScaleTransform);
			this.bladderFillAnimation.Children.Add(bladderFillScale);

			this.bladderFillAnimation.Completed += this.HandleBladderFillAnimationCompleted;
			this.bladderFillAnimation.Begin();

		}

		private void HandleBladderFillAnimationCompleted(object sender, EventArgs e) {
		}

		private void StartPeeing() {
			double scale = this.peeScaleTransform.ScaleX;
			this.bladderFillAnimation.Stop();
			
			DoubleAnimation bladderEmptyScale = new DoubleAnimation() {
				To = 0,
				From = scale,
				Duration = TimeSpan.FromSeconds(scale / this.PeeRate),
			};
			Storyboard.SetTargetProperty(bladderEmptyScale, new PropertyPath(ScaleTransform.ScaleXProperty));

			Storyboard.SetTarget(bladderEmptyScale, this.peeScaleTransform);
			this.bladderEmptyAnimation.Children.Add(bladderEmptyScale);

			this.bladderEmptyAnimation.Completed += this.HandleBladderEmptyAnimationCompleted;
			this.bladderEmptyAnimation.Begin();
		}

		private void HandleBladderEmptyAnimationCompleted(object sender, EventArgs e) {
			this.LeaveStall();
		}

		protected virtual void OnEnteredStall() {
			this.stall.PersonEntered();
			this.StartPeeing();
		}

		protected virtual void LeaveStall() {
			Storyboard sb = this.AnimateTo(new Point(-100, 500));
			this.stall.PersonLeft();
			sb.Completed += this.HandleExitCompleted;
		}

		private void HandleExitCompleted(object sender, EventArgs e) {
			this.Level.RemovePerson(this);
		}

		private void MoveTo(Point point) {
			this.translation.X = point.X;
			this.translation.Y = point.Y;
		}

		private Storyboard AnimateTo(Point point) {

			double xOffset = point.X - this.translation.X;
			double yOffset = point.Y - this.translation.Y;

			double distance = Math.Sqrt(xOffset * xOffset + yOffset * yOffset);

			
			Duration duration = new Duration(TimeSpan.FromSeconds(distance / 200));
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
	}
}
