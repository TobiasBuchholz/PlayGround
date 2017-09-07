using System;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using PlayGround.UI.Droid.Services;

namespace PlayGround.UI.Droid
{
	[Activity(Label = "PlayGround", MainLauncher = true)]
	public class MainActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_main);
            TestBillingStuffAsync();
		}

		private async Task TestBillingStuffAsync()
		{
		    var service = new AmazonIAPService();
		    bool didWork1 = await service.GetProductInfoAsync("some.product.id");
		    bool didWork2 = await service.GetProductInfoAsync("some.product.id");
		    System.Diagnostics.Debug.WriteLine($"did work: {didWork1 && didWork2}");
		}
	}
}

