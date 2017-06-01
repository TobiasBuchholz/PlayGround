using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using PlayGround.Contracts.Services.HelloWorld;
using PlayGround.Contracts.ViewModels;
using PlayGround.Models;
using ReactiveUI;
using Realms;
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

            TestSubscribeForNotificationsOnWorkerThread();
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

        private static void TestSubscribeForNotificationsOnWorkerThread()
        {
            Debug.WriteLine($"subscribe for notifications on UI thread");
            Realm
                .GetInstance()
                .All<HelloWorldModel>()
                .SubscribeForNotifications((collection, _, __) => Debug.WriteLine($"SubscribeForNotifications called on UI thread: count = {collection.Count}"));

            Observable
                .Defer(() =>
	            {
	                Debug.WriteLine($"subscribe for notifications on worker thread");
	                Realm
	                    .GetInstance()
	                    .All<HelloWorldModel>()
	                    .SubscribeForNotifications((collection, _, __) => Debug.WriteLine($"SubscribeForNotifications called on worker thread: count = {collection.Count}"));

	                return Observable.Return(Unit.Default);
	            })
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .SubscribeSafe();

            Observable
                .Defer(async () =>
	            {
	                Debug.WriteLine($"processing stuff for a few seconds");
	                await Task.Delay(TimeSpan.FromSeconds(7));

	                var realm = Realm.GetInstance();
	                realm.Write(() =>
	                {
	                    realm.Add(new HelloWorldModel { Id = realm.All<HelloWorldModel>().ToList().Count() }, true);
	                    realm.Refresh();
	                    Debug.WriteLine($"HelloWorldModel added: count = {realm.All<HelloWorldModel>().ToList().Count()}");
	                });

	                return Observable.Return(Unit.Default);
	            })
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .SubscribeSafe();
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
