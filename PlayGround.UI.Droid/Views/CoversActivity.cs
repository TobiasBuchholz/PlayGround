using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Android.App;
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

        protected override void InitViews(CompositeDisposable disposables)
        {
            _recyclerView = FindViewById<RecyclerView>(Resource.Id.activity_covers_recycler_view);
            _recyclerView.SetLayoutManager(new GridLayoutManager(this, 2));
        }

        protected override void BindGlobalControlsOnBackgroundThread(CompositeDisposable disposables)
        {
            this.WhenAnyValue(x => x.ViewModel.CoverViewModels)
                .WhereNotNull()
                .Select(CreateRecyclerAdapter)
                .ObserveOn(RxApp.MainThreadScheduler)
                .SubscribeSafe(x => _recyclerView.SetAdapter(x))
                .DisposeWith(disposables);
        }

        private CoversRecyclerAdapter CreateRecyclerAdapter(ReactiveList<ICoverViewModel> viewModels)
        {
            return new CoversRecyclerAdapter(viewModels);
        }

        protected override int LayoutResId => Resource.Layout.activity_covers;
    }
}
