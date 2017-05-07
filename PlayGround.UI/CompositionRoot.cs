using System;
using System.Reactive.Concurrency;
using System.Threading;
using Genesis.Logging;
using PlayGround.Contracts.Repositories;
using PlayGround.Contracts.Services.HelloWorld;
using PlayGround.Contracts.Services.ServerApi;
using PlayGround.Contracts.Services.SystemNotifications;
using PlayGround.Contracts.ViewModels;
using PlayGround.Repositories;
using PlayGround.Services.HelloWorld;
using PlayGround.Services.ServerApi;
using PlayGround.ViewModels;
using Realms;

namespace PlayGround.UI
{
	public abstract class CompositionRoot
	{
		// singletons
		protected readonly Lazy<IScheduler> backgroundScheduler;
		protected readonly Lazy<IScheduler> mainScheduler;
		protected readonly Lazy<ISystemNotificationsService> systemNotificationsService;
		protected readonly Lazy<IApiServiceFactory> apiServiceFactory;
		protected readonly Lazy<ICoversRepository> coversRepository;
		protected readonly Lazy<IHelloWorldService> helloWorldService;

		private readonly ILogger logger;

		protected CompositionRoot()
		{
			logger = LoggerService.GetLogger(GetType());
			backgroundScheduler = new Lazy<IScheduler>(CreateBackgroundScheduler);
			mainScheduler = new Lazy<IScheduler>(CreateMainScheduler);
			systemNotificationsService = new Lazy<ISystemNotificationsService>(this.CreateSystemNotificationsService);
			apiServiceFactory = new Lazy<IApiServiceFactory>(CreateApiServiceFactory);
			coversRepository = new Lazy<ICoversRepository>(CreateCoversRepository);
			helloWorldService = new Lazy<IHelloWorldService>(CreateHelloWorldService);
		}

		private IScheduler CreateBackgroundScheduler() =>
			LoggedCreation(() => new EventLoopScheduler());

		protected T LoggedCreation<T>(Func<T> factory)
		{
			logger.Debug("Instance of {0} requested.", typeof(T).FullName);

			using (logger.Perf("Create {0}.", typeof(T).FullName)) {
				return factory();
			}
		}

		private IScheduler CreateMainScheduler() =>
			new SynchronizationContextScheduler(SynchronizationContext.Current);

		private IApiServiceFactory CreateApiServiceFactory() =>
			LoggedCreation(() => new ApiServiceFactory());

		private ICoversRepository CreateCoversRepository() =>
			LoggedCreation(() => 
		                   new CoversRepository(
			                   () => Realm.GetInstance(),
			                   apiServiceFactory.Value));

		protected abstract ISystemNotificationsService CreateSystemNotificationsService();

		private IHelloWorldService CreateHelloWorldService() =>
			LoggedCreation(() => new HelloWorldService());

		public IMainViewModel ResolveMainViewModel() => 
			LoggedCreation(() => new MainViewModel(helloWorldService.Value));

		public ICoversViewModel ResolveCoversViewModel() =>
			LoggedCreation(() => new CoversViewModel(coversRepository.Value));

		public ISystemNotificationsService ResolveSystemNotificationsService() =>
			systemNotificationsService.Value;

		public IApiServiceFactory ResolveApiServiceFactory() => 
			apiServiceFactory.Value;

		public ICoversRepository ResolveCoversRepository() => 
			coversRepository.Value;

		public IHelloWorldService ResolveHelloWorldService() =>
			helloWorldService.Value;
	}
}
