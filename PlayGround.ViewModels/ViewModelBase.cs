using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Genesis.Logging;
using ReactiveUI;

namespace PlayGround.ViewModels
{
    public abstract class ViewModelBase : ReactiveObject, ISupportsActivation, IDisposable
    {
		private readonly ILogger _logger;
        private readonly CompositeDisposable _disposables;
        private readonly ViewModelActivator _activator;
        private bool _disposed;

        protected ViewModelBase()
        {
			_logger = LoggerService.GetLogger(GetType());
            _disposables = new CompositeDisposable();
            _activator = new ViewModelActivator();

            this.WhenActivated(disposables => {
                using(_logger.Perf("Activation")) {
                    Observable
                        .Defer(() => { InitLifeCycleAwareProperties(disposables); return Observables.Unit; })
                        .SubscribeOn(RxApp.TaskpoolScheduler)
                        .SubscribeSafe()
                        .DisposeWith(_disposables);
                }
            });
        }

        protected abstract void InitLifeCycleAwareProperties(CompositeDisposable lifeCycleDisposable);

        public ViewModelActivator Activator => _activator;
        public CompositeDisposable Disposables => _disposables;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(!_disposed) {
                if(disposing) {
                    _disposables.Dispose();
                }
            }
            _disposed = true;
        }
    }
}
