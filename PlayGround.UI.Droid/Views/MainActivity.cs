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

        protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_main);
			InitViews();
		}

		private void InitViews()
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

        protected override void BindControls(CompositeDisposable disposables)
        {
            this.OneWayBind(ViewModel, x => x.HelloWorldText, x => x._helloWorldLabel.Text)
                .DisposeWith(disposables);
        }
	}
}

