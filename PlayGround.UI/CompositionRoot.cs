using System;
using System.Reactive.Concurrency;
using System.Threading;
using Genesis.Logging;
using PCLFirebase.Contracts;
using PlayGround.Contracts.Repositories;
using PlayGround.Contracts.Services.HelloWorld;
using PlayGround.Contracts.Services.ServerApi;
using PlayGround.Contracts.Services.SystemNotifications;
using PlayGround.Contracts.Services.Web;
using PlayGround.Contracts.ViewModels;
using PlayGround.Repositories;
using PlayGround.Services.HelloWorld;
using PlayGround.Services.ServerApi;
using PlayGround.Services.Web;
using PlayGround.ViewModels;

namespace PlayGround.UI
{
	public abstract class CompositionRoot
	{
		// singletons
        protected readonly Lazy<IScheduler> _backgroundScheduler;
        protected readonly Lazy<IScheduler> _mainScheduler;
        protected readonly Lazy<ISystemNotificationsService> _systemNotificationsService;
        protected readonly Lazy<IApiServiceFactory> _apiServiceFactory;
        protected readonly Lazy<IHttpClientService> _httpClientService;
        protected readonly Lazy<ICoversRepository> _coversRepository;
        protected readonly Lazy<IHelloWorldService> _helloWorldService;
        protected readonly Lazy<IPCLFirebaseService> _firebaseService;

		private readonly ILogger logger;

		protected CompositionRoot()
		{
			logger = LoggerService.GetLogger(GetType());
			_backgroundScheduler = new Lazy<IScheduler>(CreateBackgroundScheduler);
			_mainScheduler = new Lazy<IScheduler>(CreateMainScheduler);
			_systemNotificationsService = new Lazy<ISystemNotificationsService>(this.CreateSystemNotificationsService);
			_httpClientService = new Lazy<IHttpClientService>(CreateHttpClientService);
			_apiServiceFactory = new Lazy<IApiServiceFactory>(CreateApiServiceFactory);
			_coversRepository = new Lazy<ICoversRepository>(CreateCoversRepository);
			_helloWorldService = new Lazy<IHelloWorldService>(CreateHelloWorldService);
            _firebaseService = new Lazy<IPCLFirebaseService>(CreateFirebaseService);
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

		private IHttpClientService CreateHttpClientService() =>
			LoggedCreation(() => new HttpClientService());

		private IApiServiceFactory CreateApiServiceFactory() =>
			LoggedCreation(() => new ApiServiceFactory());

		private ICoversRepository CreateCoversRepository() =>
			LoggedCreation(() => 
		                   new CoversRepository(
			                   new RealmProvider(),
			                   _apiServiceFactory.Value));

		protected abstract ISystemNotificationsService CreateSystemNotificationsService();

		private IHelloWorldService CreateHelloWorldService() =>
			LoggedCreation(() => new HelloWorldService());

        protected abstract IPCLFirebaseService CreateFirebaseService();

        public abstract IMainViewModel ResolveMainViewModel();

		public ICoversViewModel ResolveCoversViewModel() =>
			LoggedCreation(() => 
		                   new CoversViewModel(
			                   _coversRepository.Value,
			                   cover => new CoverViewModel(
				                   cover, 
				                   _httpClientService.Value)));

		public ISystemNotificationsService ResolveSystemNotificationsService() =>
			_systemNotificationsService.Value;

		public IApiServiceFactory ResolveApiServiceFactory() => 
			_apiServiceFactory.Value;

		public IHttpClientService ResolveHttpClientService() => 
			_httpClientService.Value;

		public ICoversRepository ResolveCoversRepository() => 
			_coversRepository.Value;

		public IHelloWorldService ResolveHelloWorldService() =>
			_helloWorldService.Value;

        public IPCLFirebaseService ResolveFirebaseService() =>
            _firebaseService.Value;
	}
}
