using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using PlayGround.Contracts.Services.HelloWorld;
using PlayGround.Contracts.ViewModels;
using PlayGround.Models;
using ReactiveUI;
using Splat;

namespace PlayGround.ViewModels
{
	public class MainViewModel : ReactiveObject, IMainViewModel
	{
		private readonly CompositeDisposable disposables;
		private readonly IHelloWorldService helloWorldService;

		private ObservableAsPropertyHelper<string> helloWorldText;
		private ReactiveCommandBase<Unit, HelloWorldModel> loadHelloWorld;

		public MainViewModel(IHelloWorldService helloWorldService)
		{
			this.disposables = new CompositeDisposable();
			this.helloWorldService = helloWorldService ?? Locator.Current.GetService<IHelloWorldService>();

			InitCommands();
			InitProperties();
			ExecuteCommands();
		}

		private void InitCommands()
		{
			loadHelloWorld = ReactiveCommand.CreateFromObservable(() => helloWorldService.GetHelloWorld());
			loadHelloWorld.LogThrownExceptions(disposables);
		}

		private void InitProperties()
		{
			this.WhenAnyObservable(x => x.loadHelloWorld)
			    .Select(x => x.Name)
			    .ToProperty(this, x => x.HelloWorldText, out helloWorldText)
			    .DisposeWith(disposables);
		}

		private void ExecuteCommands()
		{
			loadHelloWorld.ExecuteNow(disposables);
		}

		#region properties
		public string HelloWorldText => helloWorldText.Value;
 		#endregion

		public void Dispose()
		{
			disposables.Dispose();
		}
	}
}
