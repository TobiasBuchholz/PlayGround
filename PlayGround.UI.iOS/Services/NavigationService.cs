using System;
using PlayGround.Contracts.Services.Navigation;
using PlayGround.UI.iOS.Views;
using UIKit;

namespace PlayGround.UI.iOS.Services
{
    public sealed class NavigationService : INavigationService
    {
        private readonly UINavigationController _navigationController;

        public NavigationService(UINavigationController navigationController)
        {
            _navigationController = navigationController;
        }

        public void NavigateToCovers()
        {
            _navigationController.PushViewController(new CoversViewController(), true);
        }
    }
}
