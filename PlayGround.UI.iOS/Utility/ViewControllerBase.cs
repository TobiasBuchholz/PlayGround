using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ReactiveUI;
using Splat;

namespace PlayGround.UI.iOS.Utility
{
	public abstract class ViewControllerBase<TViewModel> : ReactiveViewController, IViewFor<TViewModel>
        where TViewModel : class
    {
        private TViewModel _viewModel;
        private CompositeDisposable _disposables;
        private ISubject<CompositeDisposable> _activated;

		protected ViewControllerBase(IntPtr handle) 
			: base(handle)
		{
            Initialize();
		}

        protected ViewControllerBase()
        {
            Initialize();
        }

        private void Initialize()
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
                    .Defer(() =>
                    {
                        ViewModel = CreateViewModel();
                        return Observables.Unit;
                    })
                .SubscribeOn(RxApp.TaskpoolScheduler);
        }

        protected virtual TViewModel CreateViewModel()
        {
            return Locator.Current.GetService<TViewModel>();
        }

        protected abstract void BindControls(CompositeDisposable disposables);

        public TViewModel ViewModel
        {
            get { return _viewModel; }
            set { this.RaiseAndSetIfChanged(ref _viewModel, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (TViewModel)value; }
        }

        protected CompositeDisposable Disposables => _disposables;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing) {
                _disposables.Dispose();
            }
        }
    }
}