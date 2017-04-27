using System;
using System.Reactive.Disposables;
using PlayGround.Contracts.ViewModels;
using PlayGround.UI.iOS.Utility;
using PlayGround.UI.iOS.Views;
using PlayGround.ViewModels;
using ReactiveUI;
using UIKit;

namespace PlayGround.UI.iOS
{
	public partial class MainViewController : ViewControllerBase<IMainViewModel>
    {
        public MainViewController(IntPtr handle) 
			: base(handle)
        {
        }

		public override void LoadView()
		{
			base.LoadView();

			var activityIndicatorView = ControlFactory
				.CreateActivityIndicator()
				.DisposeWith(Disposables);
			activityIndicatorView.StartAnimating();
			activityIndicatorView.Color = UIColor.Red;

			var bottomLabel = ControlFactory
				.CreateLabel()
				.DisposeWith(Disposables);
			bottomLabel.Text = "Text at bottom";
			bottomLabel.TextAlignment = UITextAlignment.Left;

			var aboveBottomLabel = ControlFactory
				.CreateLabel()
				.DisposeWith(Disposables);
			aboveBottomLabel.Text = "Text above bottom label";
			aboveBottomLabel.TextAlignment = UITextAlignment.Left;

			var imageView = ControlFactory
				.CreateImage()
				.DisposeWith(Disposables);
			imageView.BackgroundColor = UIColor.Yellow;
			imageView.Alpha = 0.5f;

			View.ConstrainLayout(() =>
			                     activityIndicatorView.CenterX() == View.CenterX() &&
			                     activityIndicatorView.CenterY() == View.CenterY() && 
			                     bottomLabel.Left() == View.Left() + Layout.StandardSuperviewSpacing &&
			                     bottomLabel.Right() == View.Right() - Layout.StandardSuperviewSpacing &&
			                     bottomLabel.Bottom() == View.Bottom() - Layout.StandardSuperviewSpacing &&
			                     aboveBottomLabel.Left() == bottomLabel.Left() &&
			                     aboveBottomLabel.Right() == bottomLabel.Right() &&
			                     aboveBottomLabel.Bottom() == bottomLabel.Top() &&
			                     imageView.Left() == View.Left() + Layout.StandardSuperviewSpacing &&
			                     imageView.Right() == View.Right() - Layout.StandardSuperviewSpacing &&
			                     imageView.Top() == View.Top() + Layout.StandardSuperviewSpacing &&
			                     imageView.Bottom() == aboveBottomLabel.Top()
			                    );

			View.AddSubviews(imageView, bottomLabel, aboveBottomLabel, activityIndicatorView);
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			InitViews();
			BindViewsToViewModel();
		}

		private void InitViews()
		{
			View.BringSubviewToFront(HelloWorldLabel);
			HelloWorldLabel.UserInteractionEnabled = true;
			HelloWorldLabel.AddGestureRecognizer(new UITapGestureRecognizer(() => {
				var view = new CoversViewController(new CoversViewModel());
				PresentViewController(view, true, null);
			}));
		}

		private void BindViewsToViewModel()
		{
			this.OneWayBind(ViewModel, x => x.HelloWorldText, x => x.HelloWorldLabel.Text)
			    .DisposeWith(Disposables);
		}
    }
}