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
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Reactive.Concurrency;
using System.Threading;

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
            private readonly IScheduler _mainScheduler;

            private ImageViewAsync ImageView { get; set; }
			private TextView TitleLabel { get; set; }
            private TextView PropertyLabel1 { get; set; }
            private TextView PropertyLabel2 { get; set; }
            private TextView PropertyLabel3 { get; set; }
            private TextView PropertyLabel4 { get; set; }
            private TextView PropertyLabel5 { get; set; }
            private TextView PropertyLabel6 { get; set; }
            private TextView PropertyLabel7 { get; set; }

			public CoversRecyclerViewHolder(View itemView)
				: base(itemView)
			{
				_disposables = new CompositeDisposable();
                _mainScheduler = new SynchronizationContextScheduler(SynchronizationContext.Current);

				this.WireUpControls();

                Task.Run(() => {
                    this.WhenAnyValue(x => x.ViewModel.Title)
                        .ObserveOn(_mainScheduler)
                        //.BindTo(this, x => x.TitleLabel.Text)
                        .Subscribe(x => TitleLabel.Text = x)
                        .DisposeWith(_disposables);

                    this.WhenAnyValue(x => x.ViewModel.ImageUrl)
                        .ObserveOn(_mainScheduler)
	                    .SubscribeSafe(x => 
	                    {
	                        ImageService
	                            .Instance
	                            .LoadUrl(x)
                                .DownSampleInDip(200, 300)
	                            .Into(ImageView);
	                    })
	                    .DisposeWith(_disposables);

                    this.WhenAnyValue(x => x.ViewModel.Property1)
                        .ObserveOn(_mainScheduler)
                        .Subscribe(x => PropertyLabel1.Text = x)
                        //.BindTo(this, x => x.PropertyLabel1.Text)
                        //.Subscribe(x => System.Diagnostics.Debug.WriteLine(x))
	                    .DisposeWith(_disposables);

                    this.WhenAnyValue(x => x.ViewModel.Property2)
                        .ObserveOn(_mainScheduler)
                        .Subscribe(x => PropertyLabel2.Text = x)
	                    //.BindTo(this, x => x.PropertyLabel2.Text)
                        //.Subscribe(x => System.Diagnostics.Debug.WriteLine(x))
	                    .DisposeWith(_disposables);

                    this.WhenAnyValue(x => x.ViewModel.Property3)
                        .ObserveOn(_mainScheduler)
                        .Subscribe(x => PropertyLabel3.Text = x)
	                    //.BindTo(this, x => x.PropertyLabel3.Text)
                        //.Subscribe(x => System.Diagnostics.Debug.WriteLine(x))
	                    .DisposeWith(_disposables);

                    //this.WhenAnyValue(x => x.ViewModel.Property4)
	                   // .ObserveOn(_mainScheduler)
                    //    .Subscribe(x => PropertyLabel4.Text = x)
	                   // //.BindTo(this, x => x.PropertyLabel4.Text)
                    //    //.Subscribe(x => System.Diagnostics.Debug.WriteLine(x))
	                   // .DisposeWith(_disposables);

                    //this.WhenAnyValue(x => x.ViewModel.Property5)
	                   // .ObserveOn(_mainScheduler)
                    //    .Subscribe(x => PropertyLabel5.Text = x)
	                   // //.BindTo(this, x => x.PropertyLabel5.Text)
                    //    //.Subscribe(x => System.Diagnostics.Debug.WriteLine(x))
	                   // .DisposeWith(_disposables);

                    //this.WhenAnyValue(x => x.ViewModel.Property6)
	                   // .ObserveOn(_mainScheduler)
                    //    .Subscribe(x => PropertyLabel6.Text = x)
	                   // //.BindTo(this, x => x.PropertyLabel6.Text)
                    //    //.Subscribe(x => System.Diagnostics.Debug.WriteLine(x))
	                   // .DisposeWith(_disposables);

                    //this.WhenAnyValue(x => x.ViewModel.Property7)
	                    //.ObserveOn(_mainScheduler)
                     //   .Subscribe(x => PropertyLabel7.Text = x)
	                    ////.BindTo(this, x => x.PropertyLabel7.Text)
                     //   //.Subscribe(x => System.Diagnostics.Debug.WriteLine(x))
	                    //.DisposeWith(_disposables);
                });
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
