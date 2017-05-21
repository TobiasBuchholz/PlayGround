using PCLFirebase.Contracts;
using PCLFirebase.iOS;
using PlayGround.Contracts.Services.SystemNotifications;
using PlayGround.Services.iOS.SystemNotifications;

namespace PlayGround.UI.iOS
{
	public sealed class iOSCompositionRoot : CompositionRoot
	{
		public iOSCompositionRoot()
		{
		}

		protected override ISystemNotificationsService CreateSystemNotificationsService() =>
			new SystemNotificationsService();

        protected override IPCLFirebaseService CreateFirebaseService() =>
            new PCLFirebaseService();
	}
}
