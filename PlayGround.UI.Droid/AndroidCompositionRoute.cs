using System;
using PlayGround.Contracts.Services.Navigation;
using PlayGround.Contracts.Services.SystemNotifications;
using PlayGround.UI.Droid.Services;
using Plugin.CurrentActivity;

namespace PlayGround.UI.Droid
{
	public sealed class AndroidCompositionRoute : CompositionRoot
	{
		public AndroidCompositionRoute()
		{
		}

        protected override INavigationService CreateNavigationService() => 
            new NavigationService(CrossCurrentActivity.Current);

        protected override ISystemNotificationsService CreateSystemNotificationsService()
		{
			return null;
		}
	}
}
