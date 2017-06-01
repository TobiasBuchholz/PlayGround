using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using PlayGround.Contracts.ViewModels;
using PlayGround.Models;
using ReactiveUI;
using Realms;

namespace PlayGround.ViewModels
{
	public class MainViewModel : ReactiveObject, IMainViewModel
	{
		public MainViewModel()
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
	                await Task.Delay(TimeSpan.FromSeconds(5));

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
	}
}
