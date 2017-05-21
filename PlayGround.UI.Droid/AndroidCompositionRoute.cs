using PCLFirebase.Contracts;
using PCLFirebase.Droid;
using PlayGround.Contracts.Services.SystemNotifications;

namespace PlayGround.UI.Droid
{
	public sealed class AndroidCompositionRoute : CompositionRoot
	{
		public AndroidCompositionRoute()
		{
		}

		protected override ISystemNotificationsService CreateSystemNotificationsService() => 
            null;

        protected override IPCLFirebaseService CreateFirebaseService() =>
            new PCLFirebaseService(
                PGApplication.Instance, 
                "1:537235599720:android:b42edecada2a1025", 
                "https://playground-24cec.firebaseio.com/");
	}
}
