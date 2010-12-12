using System.Windows.Controls;
using System.Windows;

namespace HoldItCore {
	public class ScoreAlert : Control {
		public ScoreAlert() {
			this.DefaultStyleKey = typeof(ScoreAlert);
		}


		public override void OnApplyTemplate() {
			base.OnApplyTemplate();

			VisualStateGroup alertStates = (VisualStateGroup)this.GetTemplateChild("AlertStates");
			alertStates.CurrentStateChanged += this.HandleAlertStateChanged;
		}

		private void HandleAlertStateChanged(object sender, VisualStateChangedEventArgs e) {
			if (e.NewState.Name == "Float")
				VisualStateManager.GoToState(this, "Steady", true);
		}

		public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(ScoreAlert), new PropertyMetadata(default(string)));
		public string Description {
			get { return (string)this.GetValue(ScoreAlert.DescriptionProperty); }
			set { this.SetValue(ScoreAlert.DescriptionProperty, value); }
		}



		public static readonly DependencyProperty AlertProperty = DependencyProperty.Register("Alert", typeof(object), typeof(ScoreAlert), new PropertyMetadata(default(string), ScoreAlert.HandleAlertChanged));
		public object Alert {
			get { return this.GetValue(ScoreAlert.AlertProperty); }
			set { this.SetValue(ScoreAlert.AlertProperty, value); }
		}

		private static void HandleAlertChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
			((ScoreAlert)sender).OnAlertChanged(e);
		}

		protected virtual void OnAlertChanged(DependencyPropertyChangedEventArgs e) {
			double value = 0;
			if (this.Alert is int)
				value = (int)this.Alert;
			else if (this.Alert is double)
				value = (double)this.Alert;

			if (value > 0)
				VisualStateManager.GoToState(this, "Plus", true);
			else
				VisualStateManager.GoToState(this, "Minus", true);
			
			VisualStateManager.GoToState(this, "Float", true);
		}

		

		
	}
}
