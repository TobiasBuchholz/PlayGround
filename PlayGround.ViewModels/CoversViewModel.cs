using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using PlayGround.Contracts.Repositories;
using PlayGround.Contracts.ViewModels;
using PlayGround.Models;
using ReactiveUI;

namespace PlayGround.ViewModels
{
	public delegate ICoverViewModel CoverViewModelFactory(Cover cover);

    public class CoversViewModel : ViewModelBase, ICoversViewModel
	{
        private readonly CoverViewModelFactory _coverViewModelFactory;
        private readonly ICoversRepository _coversRepository;
        private readonly ReactiveList<ICoverViewModel> _coversViewModels;

		public CoversViewModel(
			ICoversRepository coversRepository,
			CoverViewModelFactory coverViewModelFactory)
		{
            _coverViewModelFactory = coverViewModelFactory;
			_coversRepository = coversRepository;
            _coversViewModels = new ReactiveList<ICoverViewModel>(); 
        }

        protected override void InitLifeCycleAwareProperties(CompositeDisposable lifeCycleDisposable)
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
                .DisposeWith(lifeCycleDisposable);

            _coversRepository
                .UpdateCovers()
                .SubscribeSafe()
                .DisposeWith(lifeCycleDisposable);
        }

        public ReactiveList<ICoverViewModel> CoverViewModels => _coversViewModels;
	}
}
