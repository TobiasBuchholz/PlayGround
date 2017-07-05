using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Android.OS;
using ReactiveUI;
using ReactiveUI.AndroidSupport;
using Splat;

namespace PlayGround.UI.Droid
{
    public abstract class ActivityBase<TViewModel> : ReactiveAppCompatActivity<TViewModel>
        where TViewModel : class, IDisposable
    {
        public const string IntentKeyAutoCreateViewModel = "intent_key_auto_create_view_model";

        private CompositeDisposable _disposables;
        private ISubject<CompositeDisposable> _activated;
        private bool _autoCreateViewModel;

        protected ActivityBase()
        {
            InitializeFields();
        }

        private void InitializeFields()
        {
            _disposables = new CompositeDisposable();
            _activated = new Subject<CompositeDisposable>();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(LayoutResId);
            _autoCreateViewModel = Intent == null || Intent.Extras == null || Intent.Extras.GetBoolean(IntentKeyAutoCreateViewModel, true);

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

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Task.Run(() => Dispose());
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _disposables?.Dispose();
            ViewModel?.Dispose();
        }

        protected abstract int LayoutResId { get; }
    }
}
