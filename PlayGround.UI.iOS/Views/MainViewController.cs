using System;
using PlayGround.Contracts.ViewModels;
using ReactiveUI;

namespace PlayGround.UI.iOS
{
	public partial class MainViewController : ViewControllerBase<IMainViewModel>
    {
        public MainViewController(IntPtr handle) 
			: base(handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			InitViews();
			BindViewsToViewModel();
		}

		private void InitViews()
		{
			
		}

		private void BindViewsToViewModel()
		{
			this.OneWayBind(ViewModel, x => x.HelloWorldText, x => x.HelloWorldLabel.Text);
		}
    }
}