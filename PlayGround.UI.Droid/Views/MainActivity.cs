using System.Reactive.Disposables;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Genesis.Logging;
using PlayGround.Contracts.ViewModels;
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
		}
	}
}

