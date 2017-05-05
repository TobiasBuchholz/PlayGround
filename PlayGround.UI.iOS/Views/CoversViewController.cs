using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CoreGraphics;
using Genesis.Logging;
using PlayGround.Contracts.ViewModels;
using ReactiveUI;
using UIKit;

namespace PlayGround.UI.iOS.Views
{
	public class CoversViewController : ViewControllerBase<ICoversViewModel>
	{
		public CoversViewController()
		{
			var logger = LoggerService.GetLogger(this.GetType());

			this.WhenActivated(disposables => {
				using (logger.Perf("Activation")) 
				{
					Observable
						.Return("")
						.Subscribe()
						.DisposeWith(disposables);
				}
			});
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			View.BackgroundColor = UIColor.Green;
			var button = new UIButton(new CGRect(100, 100, 100, 100));
			button.SetTitle("Back", UIControlState.Normal);
			button.TouchUpInside += (_,__) => DismissViewController(true, null);
			View.AddSubview(button);
		}
	}
}
