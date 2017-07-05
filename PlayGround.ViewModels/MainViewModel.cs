using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using PlayGround.Contracts.Services.HelloWorld;
using PlayGround.Contracts.ViewModels;
using PlayGround.Models;
using ReactiveUI;

namespace PlayGround.ViewModels
{
	public class MainViewModel : ViewModelBase, IMainViewModel
	{
        private readonly IHelloWorldService _helloWorldService;

        private string _helloWorldText;
        private ReactiveCommandBase<Unit, HelloWorldModel> _loadHelloWorld;

		public MainViewModel(IHelloWorldService helloWorldService)
		{
			_helloWorldService = helloWorldService;

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
        }

        public string HelloWorldText {
            get => _helloWorldText;
            private set => this.RaiseAndSetIfChanged(ref _helloWorldText, value);
        }
	}
}
