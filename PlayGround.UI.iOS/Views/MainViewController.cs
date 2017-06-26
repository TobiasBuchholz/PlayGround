using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Airbnb.Lottie;
using FFImageLoading;
using FFImageLoading.Svg.Platform;
using PlayGround.Contracts.ViewModels;
using PlayGround.UI.iOS.Controls;
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

            var iconView = new IconView();
            iconView.Icon = IconRef.Close;
            iconView.IconSize = 50;
            iconView.TintColors = new UIColor[] { UIColor.Green, UIColor.Red };

            var animationView = LOTAnimationView.AnimationNamed("newspaper");
            animationView.LoopAnimation = true;
            animationView.ContentMode = UIViewContentMode.ScaleAspectFill;
            animationView.PlayWithCompletion((animationFinished) => {
                // Do Something
            });

			View.ConstrainLayout(() =>
                iconView.Top() == View.Top() + 40 && 
                iconView.Right() == View.Right() - 20 && 
				bottomLabel.Left() == View.Left() + Layout.StandardSuperviewSpacing &&
				bottomLabel.Right() == View.Right() - Layout.StandardSuperviewSpacing &&
				bottomLabel.Bottom() == View.Bottom() - Layout.StandardSuperviewSpacing &&
				aboveBottomLabel.Left() == bottomLabel.Left() &&
				aboveBottomLabel.Right() == bottomLabel.Right() &&
				aboveBottomLabel.Bottom() == bottomLabel.Top() &&
				imageView.Left() == View.Left() + Layout.StandardSuperviewSpacing &&
				imageView.Right() == View.Right() - Layout.StandardSuperviewSpacing &&
				imageView.Top() == View.Top() + Layout.StandardSuperviewSpacing &&
				imageView.Bottom() == aboveBottomLabel.Top() &&
                                 animationView.Left() == View.Left() &&
                                 animationView.Right() == View.Right() &&
                                 animationView.Top() == View.Top() &&
                                 animationView.Bottom() == View.Bottom());

            View.AddSubviews(imageView, bottomLabel, aboveBottomLabel, iconView, animationView);
		}

        protected override void BindControls(CompositeDisposable disposables)
        {
            this.OneWayBind(ViewModel, x => x.HelloWorldText, x => x.HelloWorldLabel.Text)
               .DisposeWith(disposables);
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