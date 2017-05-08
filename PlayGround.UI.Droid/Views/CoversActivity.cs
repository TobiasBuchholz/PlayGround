using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Genesis.Logging;
using PlayGround.Contracts.ViewModels;
using PlayGround.UI.Droid.Adapters;
using ReactiveUI;

namespace PlayGround.UI.Droid.Views
{
	[Activity(Label = "Covers")]
	public class CoversActivity : ActivityBase<ICoversViewModel>
	{
		public CoversActivity()
		{
			var logger = LoggerService.GetLogger(GetType());

			this.WhenActivated(disposables => {
				using (logger.Perf("Activation")) 
				{
				}
			});
		}

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_covers);
			InitRecyclerView();
		}

		private void InitRecyclerView()
		{
			var recyclerView = FindViewById<RecyclerView>(Resource.Id.activity_covers_recycler_view);
			var adapter = new CoversRecyclerAdapter(ViewModel.CoverViewModels);
			recyclerView.SetLayoutManager(new GridLayoutManager(this, 3));
			recyclerView.SetAdapter(adapter);
		}
	}
}
