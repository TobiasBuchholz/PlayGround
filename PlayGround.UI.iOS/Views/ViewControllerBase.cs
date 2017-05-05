using System;
using System.Reactive.Disposables;
using ReactiveUI;
using Splat;

namespace PlayGround.UI.iOS
{
	// a base class for view controllers to save repetitive code
	public abstract class ViewControllerBase<TViewModel> : ReactiveViewController, IViewFor<TViewModel>
		where TViewModel : class
	{
		private readonly CompositeDisposable disposables;
		private TViewModel viewModel;

		protected ViewControllerBase() 
		{
			disposables = new CompositeDisposable();
			ViewModel = Locator.Current.GetService<TViewModel>();
		}

		protected ViewControllerBase(IntPtr handle) 
			: base(handle)
		{
			disposables = new CompositeDisposable();
			ViewModel = Locator.Current.GetService<TViewModel>();
		}

		public TViewModel ViewModel
		{
			get { return viewModel; }
			set { this.RaiseAndSetIfChanged(ref viewModel, value); }
		}

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (TViewModel)value; }
		}

		protected CompositeDisposable Disposables
		{
			get { return disposables; }
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (disposing) {
				disposables.Dispose();
			}
		}
	}
}
