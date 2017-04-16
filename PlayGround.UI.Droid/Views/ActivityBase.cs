using System.Reactive.Disposables;
using ReactiveUI.AndroidSupport;
using Splat;

namespace PlayGround.UI.Droid
{
	public abstract class ActivityBase<TViewModel> : ReactiveAppCompatActivity<TViewModel>
		where TViewModel : class
	{
		private readonly CompositeDisposable disposables;

		protected ActivityBase()
		{
			disposables = new CompositeDisposable();
			ViewModel = Locator.Current.GetService<TViewModel>();
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
