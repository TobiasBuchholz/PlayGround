using System.Reactive.Disposables;
using PlayGround.Contracts.ViewModels;
using PlayGround.UI.iOS.Utility;
using UIKit;

namespace PlayGround.UI.iOS.Views
{
	public class CoversViewController : ViewControllerBase<ICoversViewModel>
	{
		private UIActivityIndicatorView activityIndicatorView;

		public CoversViewController(ICoversViewModel coversViewModel)
		{
			ViewModel = coversViewModel;
			View.BackgroundColor = UIColor.Green;
		}

		public override void LoadView()
		{
			base.LoadView();

			activityIndicatorView = ControlFactory
				.CreateActivityIndicator()
				.DisposeWith(Disposables);
			activityIndicatorView.Color = UIColor.Red;

			View.ConstrainLayout(() =>
				activityIndicatorView.CenterX() == View.CenterX() &&
				activityIndicatorView.CenterY() == View.CenterY());

			activityIndicatorView.StartAnimating();
			View.AddSubviews(activityIndicatorView);
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
		}
	}
}
