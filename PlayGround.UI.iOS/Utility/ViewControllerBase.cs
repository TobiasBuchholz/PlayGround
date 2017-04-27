namespace PlayGround.UI.iOS.Utility
{
	using System;
	using System.Reactive.Disposables;
	using ReactiveUI;
	using Splat;

	// a base class for view controllers to save repetitive code
	public abstract class ViewControllerBase<TViewModel> : ReactiveViewController, IViewFor<TViewModel>
        where TViewModel : class
    {
        private readonly CompositeDisposable disposables;
        private TViewModel viewModel;

		protected ViewControllerBase(IntPtr handle) 
			: base(handle)
		{
			this.disposables = new CompositeDisposable();
		}

        protected ViewControllerBase()
        {
            this.disposables = new CompositeDisposable();
        }

        public TViewModel ViewModel
        {
            get { return this.viewModel; }
            set { this.RaiseAndSetIfChanged(ref this.viewModel, value); }
        }

        object IViewFor.ViewModel
        {
            get { return this.ViewModel; }
            set { this.ViewModel = (TViewModel)value; }
        }

        protected CompositeDisposable Disposables => this.disposables;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			ViewModel = Locator.Current.GetService<TViewModel>();
		}

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                this.disposables.Dispose();
            }
        }
    }
}