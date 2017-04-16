using System;
using Android.App;
using Android.Runtime;
using Splat;

namespace PlayGround.UI.Droid
{
	[Application]
	public class PGApplication : Application
	{
		public PGApplication(IntPtr handle, JniHandleOwnership transfer)
			: base(handle,transfer)
		{
		}

		public override void OnCreate()
		{
			base.OnCreate();
			new SplatRegistrar().Register(Locator.CurrentMutable, new AndroidCompositionRoute());
		}
	}
}
