using System;
using System.Reactive.Disposables;
using Genesis.Logging;
using PlayGround.Contracts.ViewModels;
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

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			InitViews();
		}

		private void InitViews()
		{
			HelloWorldLabel.UserInteractionEnabled = true;
			HelloWorldLabel.AddGestureRecognizer(new UITapGestureRecognizer(() => {
				PresentViewController(new CoversViewController(), true, null);
			}));
		}
    }
}