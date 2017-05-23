using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Genesis.Logging;
using PCLFirebase.Contracts;
using PlayGround.Contracts.Services.HelloWorld;
using PlayGround.Contracts.ViewModels;
using PlayGround.Models;
using ReactiveUI;

namespace PlayGround.ViewModels
{
	public class MainViewModel : ReactiveObject, IMainViewModel, ISupportsActivation
	{
        private readonly ILogger _logger;
        private readonly ViewModelActivator _activator;
        private readonly IHelloWorldService _helloWorldService;

		private ObservableAsPropertyHelper<string> helloWorldText;
		private ReactiveCommandBase<Unit, HelloWorldModel> loadHelloWorld;

		public MainViewModel(
            IHelloWorldService helloWorldService,
            IPCLFirebaseService firebaseService)
		{
			_logger = LoggerService.GetLogger(GetType());
			_activator = new ViewModelActivator();
			_helloWorldService = helloWorldService;

			InitCommands();
			InitProperties();

			this.WhenActivated(disposables => {
				using (_logger.Perf("Activation")) 
				{
					loadHelloWorld
						.Execute()
						.SubscribeSafe()
						.DisposeWith(disposables);	

                    firebaseService
                        .RootNode
                        .GetChild("colors")
                        .ObserveValuesChanged<Color>()
                        .DebugWriteLine()
                        .SubscribeSafe();

                    firebaseService
                        .RootNode
                        .GetChild("colors")
                        .CreateChildWithAutoId()
                        .SetValue(new Color { HexValue = "#f0f0f0"});
				}
			});
		}

		private void InitCommands()
		{
			loadHelloWorld = ReactiveCommand
				.CreateFromObservable(() => _helloWorldService.GetHelloWorld())
				.LogThrownExceptions();
		}

		private void InitProperties()
		{
			this.WhenAnyObservable(x => x.loadHelloWorld)
			    .Select(x => x.Name)
			    .ToProperty(this, x => x.HelloWorldText, out helloWorldText);
		}

		#region properties
		public ViewModelActivator Activator => _activator;
		public string HelloWorldText => helloWorldText.Value;
 		#endregion
	}
}
