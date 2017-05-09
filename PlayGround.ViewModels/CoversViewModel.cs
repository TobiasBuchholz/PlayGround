using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Genesis.Logging;
using PlayGround.Contracts.Repositories;
using PlayGround.Contracts.ViewModels;
using PlayGround.Models;
using ReactiveUI;

namespace PlayGround.ViewModels
{
	public delegate ICoverViewModel CoverViewModelFactory(Cover cover, int index);

	public class CoversViewModel : ReactiveObject, ICoversViewModel, ISupportsActivation
	{
		private readonly ILogger _logger;
		private readonly ViewModelActivator _activator;
		private readonly CoverViewModelFactory _coverViewModelFactory;
		private readonly ObservableAsPropertyHelper<ReactiveList<ICoverViewModel>> _coverViewModels;

		public CoversViewModel(
			ICoversRepository coversRepository,
			CoverViewModelFactory coverViewModelFactory)
		{
			_logger = LoggerService.GetLogger(GetType());
			_activator = new ViewModelActivator();
			_coverViewModelFactory = coverViewModelFactory;

			coversRepository
				.GetCovers()
				.Do(_ => _coverViewModels?.Value?.Clear())
				.Iterate()
				.Select((cover, index) => _coverViewModelFactory(cover, index))
				.Scan(new ReactiveList<ICoverViewModel>(), (x, y) => { x.Add(y); return x; })
				.ToProperty(this, x => x.CoverViewModels, out _coverViewModels);

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
		public ReactiveList<ICoverViewModel> CoverViewModels => _coverViewModels.Value;
	}
}
