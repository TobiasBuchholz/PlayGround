using System;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Genesis.Logging;
using PlayGround.Contracts.ViewModels;
using PlayGround.UI.iOS.Utility;
using ReactiveUI;
using UIKit;

namespace PlayGround.UI.iOS.Views
{
	public class CoversViewController : ViewControllerBase<ICoversViewModel>
	{
		private UIButton backButton;

		public CoversViewController()
		{
			View.BackgroundColor = UIColor.Green;

			var logger = LoggerService.GetLogger(this.GetType());

			this.WhenActivated(disposables => {
				using (logger.Perf("Activation")) 
				{
					this.WhenAnyValue(x => x.ViewModel.CoverViewModels)
					    .WhereNotNull()
					    .SubscribeSafe(x => Debug.WriteLine("covers " + x.Count))
					    .DisposeWith(disposables);
				}
			});
		}

		public override void LoadView()
		{
			base.LoadView();

			backButton = ControlFactory
				.CreateButton()
				.DisposeWith(Disposables);

			backButton.SetTitle("Back", UIControlState.Normal);
			backButton.TouchUpInside += (sender, e) => DismissViewController(true, null);

			View.ConstrainLayout(() =>
				backButton.CenterX() == View.CenterX() &&
				backButton.CenterY() == View.CenterY());

			View.AddSubviews(backButton);
		}
	}
}
