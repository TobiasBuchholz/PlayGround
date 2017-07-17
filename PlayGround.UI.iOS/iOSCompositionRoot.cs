using PlayGround.Contracts.Services.Navigation;
using PlayGround.Contracts.Services.SystemNotifications;
using PlayGround.Services.iOS.SystemNotifications;
using PlayGround.UI.iOS.Services;
using UIKit;

namespace PlayGround.UI.iOS
{
	public sealed class iOSCompositionRoot : CompositionRoot
	{
        private readonly UINavigationController _navigationController;

        public iOSCompositionRoot(UINavigationController navigationController)
		{
            _navigationController = navigationController;
		}

        protected override INavigationService CreateNavigationService() =>
            new NavigationService(_navigationController);

		protected override ISystemNotificationsService CreateSystemNotificationsService() =>
			new SystemNotificationsService();
	}
}
