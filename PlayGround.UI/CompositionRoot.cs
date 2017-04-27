﻿using System;
using System.Reactive.Concurrency;
using System.Threading;
using Genesis.Logging;
using PlayGround.Contracts.Services.HelloWorld;
using PlayGround.Contracts.Services.SystemNotifications;
using PlayGround.Contracts.ViewModels;
using PlayGround.Services.HelloWorld;
using PlayGround.ViewModels;

namespace PlayGround.UI
{
	public abstract class CompositionRoot
	{
		// singletons
		protected readonly Lazy<IScheduler> backgroundScheduler;
		protected readonly Lazy<IScheduler> mainScheduler;
		protected readonly Lazy<ISystemNotificationsService> systemNotificationsService;
		protected readonly Lazy<IHelloWorldService> helloWorldService;

		private readonly ILogger logger;

		protected CompositionRoot()
		{
			logger = LoggerService.GetLogger(GetType());
			backgroundScheduler = new Lazy<IScheduler>(CreateBackgroundScheduler);
			mainScheduler = new Lazy<IScheduler>(CreateMainScheduler);
			systemNotificationsService = new Lazy<ISystemNotificationsService>(this.CreateSystemNotificationsService);
			helloWorldService = new Lazy<IHelloWorldService>(CreateHelloWorldService);
		}

		private IScheduler CreateBackgroundScheduler() 
		{
			return LoggedCreation(() => new EventLoopScheduler());
		}

		protected T LoggedCreation<T>(Func<T> factory)
		{
			logger.Debug("Instance of {0} requested.", typeof(T).FullName);

			using (logger.Perf("Create {0}.", typeof(T).FullName)) {
				return factory();
			}
		}

		private IScheduler CreateMainScheduler() 
		{
			return new SynchronizationContextScheduler(SynchronizationContext.Current);
		}

		protected abstract ISystemNotificationsService CreateSystemNotificationsService();

		private IHelloWorldService CreateHelloWorldService() 
		{
			return LoggedCreation(() => new HelloWorldService());
		}

		public IMainViewModel ResolveMainViewModel() => 
			LoggedCreation(() => new MainViewModel(helloWorldService.Value));

		public ISystemNotificationsService ResolveSystemNotificationsService() =>
			systemNotificationsService.Value;

		public IHelloWorldService ResolveHelloWorldService() =>
			helloWorldService.Value;
	}
}
