using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Genesis.Logging;
using PlayGround.Contracts.ViewModels;
using PlayGround.UI.iOS.Utility;
using ReactiveUI;
using UIKit;
using Splat;

namespace PlayGround.UI.iOS.Views
{
	public class CoversViewController : ViewControllerBase<ICoversViewModel>
	{
        private UIButton _backButton;
        private UIImageView _imageView;

		public CoversViewController()
		{
			View.BackgroundColor = UIColor.Green;

			var logger = LoggerService.GetLogger(GetType());

			this.WhenActivated(disposables => {
				using (logger.Perf("Activation")) 
				{
					this.WhenAnyValue(x => x.ViewModel.CoverViewModels)
                        .SelectMany(x => x[0].WhenAnyValue(y => y.Image))
                        .WhereNotNull()
					    .ObserveOn(RxApp.MainThreadScheduler)
                        .SubscribeSafe(x => _imageView.Image = x.ToNative())
					    .DisposeWith(disposables);
				}
			});
		}

		public override void LoadView()
		{
			base.LoadView();

			_backButton = ControlFactory
				.CreateButton()
				.DisposeWith(Disposables);

			_backButton.SetTitle("Back", UIControlState.Normal);
			_backButton.TouchUpInside += (sender, e) => DismissViewController(true, null);

            _imageView = ControlFactory
                .CreateImage()
                .DisposeWith(Disposables);
            _imageView.BackgroundColor = UIColor.Orange;

			View.ConstrainLayout(() =>
				_backButton.CenterX() == View.CenterX() &&
				_backButton.CenterY() == View.CenterY() &&
	            _imageView.Left() == View.Left() && 
	            _imageView.Right() == View.Right() && 
	            _imageView.Top() == View.Top() && 
	            _imageView.Bottom() == View.Bottom());

            View.AddSubviews(_backButton, _imageView);
		}
	}
}
