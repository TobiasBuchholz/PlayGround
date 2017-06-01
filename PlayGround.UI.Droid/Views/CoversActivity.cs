using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using PlayGround.Contracts.ViewModels;
using PlayGround.UI.Droid.Adapters;
using ReactiveUI;

namespace PlayGround.UI.Droid.Views
{
	[Activity(Label = "Covers")]
	public class CoversActivity : ActivityBase<ICoversViewModel>
	{
        private RecyclerView _recyclerView;

		public CoversActivity()
		{
		}

        protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_covers);
			InitRecyclerView();
		}

		private void InitRecyclerView()
		{
            _recyclerView = FindViewById<RecyclerView>(Resource.Id.activity_covers_recycler_view);
            _recyclerView.SetLayoutManager(new GridLayoutManager(this, 2));
		}

        protected override void BindControls(CompositeDisposable disposables)
        {
            this.WhenAnyValue(x => x.ViewModel.CoverViewModels)
                .WhereNotNull()
                .Select(x => new CoversRecyclerAdapter(ViewModel.CoverViewModels))
                .SubscribeSafe(x => _recyclerView.SetAdapter(x))
                .DisposeWith(disposables);
        }
	}
}
