using Android.App;
using Android.OS;
using Android.Widget;
using PlayGround.Contracts.ViewModels;
using ReactiveUI;

namespace PlayGround.UI.Droid
{
	[Activity(Label = "PlayGround", MainLauncher = true)]
	public class MainActivity : ActivityBase<IMainViewModel>
	{
		private TextView helloWorldLabel;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_main);
			InitViews();
			BindViewsToViewModel();
		}

		private void InitViews()
		{
			helloWorldLabel = FindViewById<TextView>(Resource.Id.activity_main_hello_world_label);
		}

		private void BindViewsToViewModel()
		{
			this.OneWayBind(ViewModel, x => x.HelloWorldText, x => x.helloWorldLabel.Text);
		}
	}
}

