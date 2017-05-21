using PCLFirebase.Contracts;
using PCLFirebase.iOS;
using PlayGround.Contracts.Services.SystemNotifications;
using PlayGround.Contracts.ViewModels;
using PlayGround.Services.iOS.SystemNotifications;
using PlayGround.UI.iOS.Models;
using PlayGround.ViewModels;

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

        public override IMainViewModel ResolveMainViewModel() => 
             LoggedCreation(() => 
                            new MainViewModel(
                                _helloWorldService.Value,
                                _firebaseService.Value,
                                x => new GroceryItem(x)));
	}
}
