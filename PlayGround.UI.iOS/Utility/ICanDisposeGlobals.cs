using System;
namespace PlayGround.UI.iOS.Utility
{
    public interface ICanDisposeGlobals
    {
        void DisposeGlobalsOnMainThread();
        void DisposeGlobalsOnBackgroundThread();
    }
}
