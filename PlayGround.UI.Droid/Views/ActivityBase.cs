using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ReactiveUI;
using ReactiveUI.AndroidSupport;
using Splat;

namespace PlayGround.UI.Droid
{
	public abstract class ActivityBase<TViewModel> : ReactiveAppCompatActivity<TViewModel>
        where TViewModel : class, IDisposable
	{
        private readonly CompositeDisposable _disposables;
        private ISubject<CompositeDisposable> _activated;

		protected ActivityBase()
		{
            _disposables = new CompositeDisposable();
            _activated = new Subject<CompositeDisposable>();
            this.WhenActivated(_activated.OnNext);

            _activated
                .CombineLatest(CreateViewModelDeferred(), (disposables,_) => disposables)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Do(x => BindControls(x))
                .SubscribeSafe()
                .DisposeWith(_disposables);
		}

        private IObservable<Unit> CreateViewModelDeferred()
        {
            return Observable
                .Defer(() => { ViewModel = CreateViewModel(); return Observables.Unit;})
                .SubscribeOn(RxApp.TaskpoolScheduler);
        }

        protected virtual TViewModel CreateViewModel() => Locator.Current.GetService<TViewModel>();
        protected abstract void BindControls(CompositeDisposable disposables);
        protected CompositeDisposable Disposables => _disposables;

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Dispose();
        }

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing) {
				_disposables.Dispose();
                ViewModel.Dispose();
			}
		}
	}
}
