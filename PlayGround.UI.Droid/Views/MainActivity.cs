using System;
using System.Reactive.Disposables;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using FFImageLoading;
using FFImageLoading.Svg.Platform;
using FFImageLoading.Views;
using PlayGround.Contracts.ViewModels;
using PlayGround.UI.Droid.Views;
using Plugin.InAppBilling;
using Plugin.InAppBilling.Abstractions;
using ReactiveUI;

namespace PlayGround.UI.Droid
{
	[Activity(Label = "PlayGround", MainLauncher = true)]
	public class MainActivity : ActivityBase<IMainViewModel>
	{
        private TextView _helloWorldLabel;

        public MainActivity() 
		{
		}

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var productIds = new string []{ "managed_test_01" };

            var billing = CrossInAppBilling.Current;
            var connected = await billing.ConnectAsync();
            var items = await billing.GetProductInfoAsync(ItemType.InAppPurchase, productIds);

            System.Diagnostics.Debug.WriteLine("items " + items);
        }

        protected override void InitViews(CompositeDisposable disposables)
		{
			_helloWorldLabel = FindViewById<TextView>(Resource.Id.activity_main_hello_world_label);
			_helloWorldLabel.Click += (sender, e) => StartActivity(new Intent(this, typeof(CoversActivity)));

            var imageView = FindViewById<ImageViewAsync>(Resource.Id.activity_main_image_view);
            ImageService
                .Instance
                .LoadCompiledResource("sample.svg")
                .WithCustomDataResolver(new SvgDataResolver(0, Resources.DisplayMetrics.HeightPixels, true))
                .Into(imageView);
		}

        protected override void BindGlobalControlsOnMainThread(CompositeDisposable disposables)
        {
            this.OneWayBind(ViewModel, x => x.HelloWorldText, x => x._helloWorldLabel.Text)
                .DisposeWith(disposables);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            InAppBillingImplementation.HandleActivityResult(requestCode, resultCode, data);
        }

        protected override int LayoutResId => Resource.Layout.activity_main;
    }
}

