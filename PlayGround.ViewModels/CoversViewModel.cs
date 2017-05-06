using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Genesis.Logging;
using PlayGround.Contracts.Repositories;
using PlayGround.Contracts.ViewModels;
using ReactiveUI;

namespace PlayGround.ViewModels
{
	public class CoversViewModel : ReactiveObject, ICoversViewModel, ISupportsActivation
	{
		private readonly ILogger logger;
		private readonly ViewModelActivator activator;

		public CoversViewModel(ICoversRepository coversRepository)
		{
			this.logger = LoggerService.GetLogger(this.GetType());
			this.activator = new ViewModelActivator();

			coversRepository
				.GetCovers()
				.Do(covers => Debug.WriteLine("covers " + covers.Count()))
				.SubscribeSafe();

			this.WhenActivated(disposables => {
				using (this.logger.Perf("Activation")) 
				{
					Observable
						.Return("")
						.Subscribe()
						.DisposeWith(disposables);
				}
			});
		}

		public ViewModelActivator Activator => activator;
	}
}
