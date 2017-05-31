using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Genesis.Logging;
using PlayGround.Contracts.Repositories;
using PlayGround.Contracts.ViewModels;
using PlayGround.Models;
using ReactiveUI;

namespace PlayGround.ViewModels
{
	public delegate ICoverViewModel CoverViewModelFactory(Cover cover);

	public class CoversViewModel : ReactiveObject, ICoversViewModel, ISupportsActivation
	{
		private readonly ILogger _logger;
		private readonly ViewModelActivator _activator;
        private readonly CoverViewModelFactory _coverViewModelFactory;
        private readonly ICoversRepository _coversRepository;
        private readonly ReactiveList<ICoverViewModel> _coversViewModels;

		public CoversViewModel(
			ICoversRepository coversRepository,
			CoverViewModelFactory coverViewModelFactory)
		{
			_logger = LoggerService.GetLogger(GetType());
			_activator = new ViewModelActivator();
            _coverViewModelFactory = coverViewModelFactory;
			_coversRepository = coversRepository;
            _coversViewModels = new ReactiveList<ICoverViewModel>(); 

            this.WhenActivated(disposables => {
				using(_logger.Perf("Activation"))
                {
                    InitProperiesDeferred(disposables);
                }
            });
        }

        private void InitProperiesDeferred(CompositeDisposable disposables)
        {
            Observable
                .Defer(() => 
	            {
	                InitProperties(disposables);
	                return Observables.Unit;
	            })
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .SubscribeSafe();
        }

        private void InitProperties(CompositeDisposable disposables)
        {
            _coversRepository
	            .GetCovers()
                .Select(covers => covers.Select(x => _coverViewModelFactory(x)).ToList())
				.ObserveOn(RxApp.MainThreadScheduler)
                .SubscribeSafe(x => 
                {
	                _coversViewModels.Clear();
	                _coversViewModels.AddRange(x);
                })
                .DisposeWith(disposables);

            _coversRepository
                .UpdateCovers()
                .SubscribeSafe()
                .DisposeWith(disposables);
        }

        public ViewModelActivator Activator => _activator;
		public ReactiveList<ICoverViewModel> CoverViewModels => _coversViewModels;

	}
}
