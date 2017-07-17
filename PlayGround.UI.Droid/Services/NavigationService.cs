﻿﻿using System;
using Android.Content;
using PlayGround.Contracts.Services.Navigation;
using PlayGround.UI.Droid.Views;
using Plugin.CurrentActivity;

namespace PlayGround.UI.Droid.Services
{
    public sealed class NavigationService : INavigationService
    {
        private readonly ICurrentActivity _current;

        public NavigationService(ICurrentActivity current)
        {
            _current = current;
        }

        public void NavigateToCovers()
        {
            _current.Activity.StartActivity(new Intent(_current.Activity, typeof(CoversActivity)));
        }
    }
}
