using Genesis.Ensure;
using Genesis.Logging;
using PlayGround.Contracts.Repositories;
using PlayGround.Contracts.Services.HelloWorld;
using PlayGround.Contracts.Services.Navigation;
using PlayGround.Contracts.Services.ServerApi;
using PlayGround.Contracts.ViewModels;
using Splat;

namespace PlayGround.UI
{
	public class SplatRegistrar
	{
		private readonly Genesis.Logging.ILogger _logger;

		public SplatRegistrar()
		{
			_logger = LoggerService.GetLogger(GetType());
		}

		public void Register(IMutableDependencyResolver splatLocator, CompositionRoot compositionRoot)
		{
			Ensure.ArgumentNotNull(splatLocator, nameof(splatLocator));

			using (_logger.Perf("Registration.")) {
				RegisterServices(splatLocator, compositionRoot);
				RegisterRepositories(splatLocator, compositionRoot);
				RegisterViewModels(splatLocator, compositionRoot);
			}
		}

		private void RegisterServices(IMutableDependencyResolver splatLocator, CompositionRoot compositionRoot)
		{
			splatLocator.RegisterConstant(compositionRoot.ResolveApiServiceFactory(), typeof(IApiServiceFactory));
            splatLocator.RegisterConstant(compositionRoot.ResolveNavigationService(), typeof(INavigationService));
            splatLocator.RegisterConstant(compositionRoot.ResolveHelloWorldService(), typeof(IHelloWorldService));
		}

		private void RegisterRepositories(IMutableDependencyResolver splatLocator, CompositionRoot compositionRoot)
		{
			splatLocator.RegisterConstant(compositionRoot.ResolveCoversRepository(), typeof(ICoversRepository));
		}

		private void RegisterViewModels(IMutableDependencyResolver splatLocator, CompositionRoot compositionRoot)
		{
			splatLocator.Register(compositionRoot.ResolveMainViewModel, typeof(IMainViewModel));
			splatLocator.Register(compositionRoot.ResolveCoversViewModel, typeof(ICoversViewModel));
		}
	}
}
