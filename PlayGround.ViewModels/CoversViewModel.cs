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
		private readonly IReactiveList<Cover> _covers;

		public CoversViewModel(
			ICoversRepository coversRepository,
			CoverViewModelFactory coverViewModelFactory)
		{
			_logger = LoggerService.GetLogger(GetType());
			_activator = new ViewModelActivator();
			_coverViewModelFactory = coverViewModelFactory;
            _covers = new ReactiveList<Cover>();

            coversRepository
                .GetCovers()
                .SubscribeSafe(x =>
	            {
	                _covers.Clear();
	                _covers.AddRange(x);
	            });

			this.WhenActivated(disposables => {
				using(_logger.Perf("Activation")) 
				{
					coversRepository
						.UpdateCovers()
						.SubscribeSafe()
						.DisposeWith(disposables);
				}
			});
		}

		public ViewModelActivator Activator => _activator;
		public IReactiveDerivedList<ICoverViewModel> CoverViewModels => 
            _covers.CreateDerivedCollection(x => _coverViewModelFactory(x));
	}
}
