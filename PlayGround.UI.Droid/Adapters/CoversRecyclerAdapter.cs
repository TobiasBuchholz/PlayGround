using System;
using System.Reactive.Disposables;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using PlayGround.Contracts.ViewModels;
using ReactiveUI;
using ReactiveUI.Android.Support;
using FFImageLoading;
using FFImageLoading.Views;

namespace PlayGround.UI.Droid.Adapters
{
	public class CoversRecyclerAdapter : ReactiveRecyclerViewAdapter<ICoverViewModel>
	{
		public CoversRecyclerAdapter(IReadOnlyReactiveList<ICoverViewModel> backingList)
			: base(backingList)
		{
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.layout_cover_item, parent, false);
			return new CoversRecyclerViewHolder(view);
		}

		private class CoversRecyclerViewHolder : ReactiveRecyclerViewViewHolder<ICoverViewModel>
		{
			private readonly CompositeDisposable _disposables;
			private TextView TitleLabel { get; set; }
            private ImageViewAsync ImageView { get; set; }

			public CoversRecyclerViewHolder(View itemView)
				: base(itemView)
			{
				_disposables = new CompositeDisposable();
				this.WireUpControls();

				this.WhenAnyValue(x => x.ViewModel.Title)
				    .SubscribeSafe(x => TitleLabel.Text = x)
				    .DisposeWith(_disposables);

                this.WhenAnyValue(x => x.ViewModel.ImageUrl)
                    .SubscribeSafe(x => 
                {
                    ImageService
                        .Instance
                        .LoadUrl(x)
                        .Into(ImageView);
                })
                    .DisposeWith(_disposables);
			}

			protected override void Dispose(bool disposing)
			{
				base.Dispose(disposing);
				if(disposing) {
					_disposables.Dispose();
				}
			}
		}
	}
}
