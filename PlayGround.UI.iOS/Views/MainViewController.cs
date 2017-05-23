using System;
using System.Reactive.Disposables;
using FFImageLoading;
using FFImageLoading.Svg.Platform;
using Genesis.Logging;
using PlayGround.Contracts.ViewModels;
using PlayGround.UI.iOS.Utility;
using PlayGround.UI.iOS.Views;
using ReactiveUI;
using UIKit;

namespace PlayGround.UI.iOS
{
	public partial class MainViewController : ViewControllerBase<IMainViewModel>
    {
        public MainViewController(IntPtr handle) 
			: base(handle)
        {
			var logger = LoggerService.GetLogger(this.GetType());

			this.WhenActivated(disposables => {
				using (logger.Perf("Activation")) 
				{
					this.OneWayBind(ViewModel, x => x.HelloWorldText, x => x.HelloWorldLabel.Text)
					    .DisposeWith(disposables);
				}
			});
        }

		public override void LoadView()
		{
			base.LoadView();

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
            imageView.ContentMode = UIViewContentMode.ScaleAspectFill;

            ImageService
                .Instance
                .LoadFile("Images/sample.svg")
                .WithCustomDataResolver(new SvgDataResolver(0, (int) View.Frame.Height, true))
                .Into(imageView);

			View.ConstrainLayout(() =>
				bottomLabel.Left() == View.Left() + Layout.StandardSuperviewSpacing &&
				bottomLabel.Right() == View.Right() - Layout.StandardSuperviewSpacing &&
				bottomLabel.Bottom() == View.Bottom() - Layout.StandardSuperviewSpacing &&
				aboveBottomLabel.Left() == bottomLabel.Left() &&
				aboveBottomLabel.Right() == bottomLabel.Right() &&
				aboveBottomLabel.Bottom() == bottomLabel.Top() &&
				imageView.Left() == View.Left() + Layout.StandardSuperviewSpacing &&
				imageView.Right() == View.Right() - Layout.StandardSuperviewSpacing &&
				imageView.Top() == View.Top() + Layout.StandardSuperviewSpacing &&
				imageView.Bottom() == aboveBottomLabel.Top());

			View.AddSubviews(imageView, bottomLabel, aboveBottomLabel);
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			InitViews();
		}

		private void InitViews()
		{
			View.BringSubviewToFront(HelloWorldLabel);
			HelloWorldLabel.UserInteractionEnabled = true;
			HelloWorldLabel.AddGestureRecognizer(new UITapGestureRecognizer(() => {
				var view = new CoversViewController();
				PresentViewController(view, true, null);
			}));
		}
    }
}