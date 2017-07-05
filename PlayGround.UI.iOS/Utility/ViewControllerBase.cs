using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using ReactiveUI;
using Splat;
using UIKit;

namespace PlayGround.UI.iOS.Utility
{
    public abstract class ViewControllerBase<TViewModel> : ReactiveViewController<TViewModel>, ICanDisposeGlobals
        where TViewModel : class, IDisposable
    {
        private CompositeDisposable _disposables;
        private ISubject<CompositeDisposable> _activated;
        private bool _autoCreateViewModel;

        #region constructors
        protected ViewControllerBase(IntPtr handle, bool autoCreateViewModel = true) 
            : base(handle)
        {
            _autoCreateViewModel = autoCreateViewModel;
            InitializeFields();
        }

        private void InitializeFields()
        {
            _disposables = new CompositeDisposable();
            _activated = new Subject<CompositeDisposable>();
        }

        protected ViewControllerBase(bool autoCreateViewModel = true)
        {
            _autoCreateViewModel = autoCreateViewModel;
            InitializeFields();
        }
        #endregion

        public override void LoadView()
        {
            base.LoadView();
            InitViews(_disposables);
            InitViewModel();
        }

        protected abstract void InitViews(CompositeDisposable disposables);

        private void InitViewModel()
        {
            this.WhenActivated(_activated.OnNext);

            _activated
                .CombineLatest(GetInitializedViewModel(), (disposables,_) => disposables)
                .Do(x => RxApp.MainThreadScheduler.Schedule(() => BindLifecycleControlsOnMainThread(x)))
                .Do(x => RxApp.TaskpoolScheduler.Schedule(() => BindLifecycleControlsOnBackgroundThread(x)))
                .SubscribeSafe()
                .DisposeWith(_disposables);

            if(_autoCreateViewModel) {
                Task.Run(() => ViewModel = CreateViewModel());
            }  
        }

        private IObservable<TViewModel> GetInitializedViewModel()
        {
            return this
                .WhenAnyValue(x => x.ViewModel)
                .WhereNotNull()
                .Do(_ => RxApp.MainThreadScheduler.Schedule(() => BindGlobalControlsOnMainThread(_disposables)))
                .Do(_ => RxApp.TaskpoolScheduler.Schedule(() => BindGlobalControlsOnBackgroundThread(_disposables)));
        }

        protected virtual void BindGlobalControlsOnMainThread(CompositeDisposable disposables)
        {
            // override if needed
        }

        protected virtual void BindGlobalControlsOnBackgroundThread(CompositeDisposable disposables)
        {
            // override if needed
        }

        protected virtual void BindLifecycleControlsOnMainThread(CompositeDisposable disposables)
        {
            // override if needed
        }

        protected virtual void BindLifecycleControlsOnBackgroundThread(CompositeDisposable disposables) 
        {
            // override if needed
        }

        protected virtual TViewModel CreateViewModel() => Locator.Current.GetService<TViewModel>();

        public override void WillMoveToParentViewController(UIViewController parent)
        {
            base.WillMoveToParentViewController(parent);
            if(parent == null && ViewModel != null) {
                RxApp.MainThreadScheduler.Schedule(() => DisposeGlobalsOnMainThread());
                RxApp.TaskpoolScheduler.Schedule(() => DisposeGlobalsOnBackgroundThread());
            }
        }

        public virtual void DisposeGlobalsOnMainThread()
        {
            View.Subviews.DisposeGlobals();
        }

        public virtual void DisposeGlobalsOnBackgroundThread()
        {
            _disposables?.Dispose();
            ViewModel?.Dispose();            
        }
    }
}