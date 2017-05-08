using PlayGround.Contracts.Services.SystemNotifications;

namespace PlayGround.UI.Droid
{
	public sealed class AndroidCompositionRoute : CompositionRoot
	{
		public AndroidCompositionRoute()
		{
		}

		protected override ISystemNotificationsService CreateSystemNotificationsService()
		{
			return null;
		}
	}
}
