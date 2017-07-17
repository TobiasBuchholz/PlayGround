using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using PlayGround.Contracts.Services.HelloWorld;
using PlayGround.Contracts.Services.Navigation;
using PlayGround.Contracts.ViewModels;
using PlayGround.Models;
using ReactiveUI;

namespace PlayGround.ViewModels
{
	public class MainViewModel : ViewModelBase, IMainViewModel
	{
        private readonly IHelloWorldService _helloWorldService;
        private readonly INavigationService _navigationService;

        private string _helloWorldText;
        private ReactiveCommandBase<Unit, HelloWorldModel> _loadHelloWorld;

		public MainViewModel(
            IHelloWorldService helloWorldService,
            INavigationService navigationService)
		{
			_helloWorldService = helloWorldService;
            _navigationService = navigationService;

            _loadHelloWorld = ReactiveCommand
                .CreateFromObservable(() => _helloWorldService.GetHelloWorld())
                .LogThrownExceptions(Disposables)
                .DisposeWith(Disposables);

            _loadHelloWorld
                .Execute()
                .SubscribeSafe()
                .DisposeWith(Disposables);
		}

        protected override void InitLifeCycleAwareProperties(CompositeDisposable lifeCycleDisposable)
        {
            this.WhenAnyObservable(x => x._loadHelloWorld)
                .Select(x => x.Name)
                .BindTo(this, x => x.HelloWorldText)
                .DisposeWith(lifeCycleDisposable);

            this.WhenAnyObservable(x => x._loadHelloWorld)
                .Where(x => x.Name == "Hello world! 10")
                .Do(_ => _navigationService.NavigateToCovers())
                .SubscribeSafe()
                .DisposeWith(lifeCycleDisposable);
        }

        public string HelloWorldText {
            get => _helloWorldText;
            private set => this.RaiseAndSetIfChanged(ref _helloWorldText, value);
        }
	}
}
