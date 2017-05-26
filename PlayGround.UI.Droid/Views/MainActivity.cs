using System.IO;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using FFImageLoading;
using FFImageLoading.Svg.Platform;
using FFImageLoading.Views;
using Genesis.Logging;
using PlayGround.Contracts.ViewModels;
using PlayGround.UI.Droid.Controls;
using PlayGround.UI.Droid.Views;
using ReactiveUI;

namespace PlayGround.UI.Droid
{
	[Activity(Label = "PlayGround", MainLauncher = true)]
	public class MainActivity : ActivityBase<IMainViewModel>
	{
		private TextView helloWorldLabel;

		public MainActivity() 
		{
			var logger = LoggerService.GetLogger(this.GetType());

			this.WhenActivated(disposables => {
				using (logger.Perf("Activation")) 
				{
					this.OneWayBind(ViewModel, x => x.HelloWorldText, x => x.helloWorldLabel.Text)
					    .DisposeWith(disposables);
				}
			});
		}

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_main);
			InitViews();
		}

		private void InitViews()
		{
			helloWorldLabel = FindViewById<TextView>(Resource.Id.activity_main_hello_world_label);
			helloWorldLabel.Click += (sender, e) => StartActivity(new Intent(this, typeof(CoversActivity)));

            var imageView = FindViewById<ImageViewAsync>(Resource.Id.activity_main_image_view);
            ImageService
                .Instance
                .LoadCompiledResource("sample.svg")
                .WithCustomDataResolver(new SvgDataResolver(0, Resources.DisplayMetrics.HeightPixels, true))
                .Into(imageView);
		}
	}
}

