using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Genesis.Logging;
using PlayGround.Contracts.Services.HelloWorld;
using PlayGround.Contracts.ViewModels;
using PlayGround.Models;
using ReactiveUI;

namespace PlayGround.ViewModels
{
	public class MainViewModel : ReactiveObject, IMainViewModel, ISupportsActivation
	{
		private readonly ILogger logger;
		private readonly ViewModelActivator activator;
		private readonly IHelloWorldService helloWorldService;

		private ObservableAsPropertyHelper<string> helloWorldText;
		private ReactiveCommandBase<Unit, HelloWorldModel> loadHelloWorld;

		public MainViewModel(IHelloWorldService helloWorldService)
		{
			this.logger = LoggerService.GetLogger(this.GetType());
			this.activator = new ViewModelActivator();
			this.helloWorldService = helloWorldService;

			InitCommands();
			InitProperties();

			this.WhenActivated(disposables => {
				using (this.logger.Perf("Activation")) 
				{
					loadHelloWorld
						.Execute()
						.SubscribeSafe()
						.DisposeWith(disposables);	
				}
			});
		}

		private void InitCommands()
		{
			loadHelloWorld = ReactiveCommand
				.CreateFromObservable(() => helloWorldService.GetHelloWorld())
				.LogThrownExceptions();
		}

		private void InitProperties()
		{
			this.WhenAnyObservable(x => x.loadHelloWorld)
			    .Select(x => x.Name)
			    .ToProperty(this, x => x.HelloWorldText, out helloWorldText);
		}

		#region properties
		public ViewModelActivator Activator => activator;
		public string HelloWorldText => helloWorldText.Value;
 		#endregion
	}
}
