using System.Reactive.Disposables;
using System.Reactive.Linq;
using PlayGround.Contracts.ViewModels;
using PlayGround.UI.iOS.Utility;
using ReactiveUI;
using UIKit;
using Xamarin.Forms;

namespace PlayGround.UI.iOS.Views
{
	public class CoversViewController : ViewControllerBase<ICoversViewModel>
	{
        private UIButton _backButton;
        private UIButton _detailsButton;
        private UICollectionView _collectionView;

		public CoversViewController()
		{
		}

		public override void LoadView()
		{
			base.LoadView();

			_backButton = ControlFactory
				.CreateButton()
				.DisposeWith(Disposables);

			_backButton.SetTitle("Back", UIControlState.Normal);
			_backButton.TouchUpInside += (_,__) => DismissViewController(true, null);

            _detailsButton = ControlFactory
                .CreateButton()
                .DisposeWith(Disposables);

            _detailsButton.SetTitle("Details", UIControlState.Normal);
            _detailsButton.TouchUpInside += (_,__) => {
                var page = new DetailsPage();
                PresentViewController(page.CreateViewController(), true, null);
            };

            _collectionView = new UICollectionView(View.Frame, CreateCollectionViewLayout());
            _collectionView.RegisterClassForCell(typeof(CoverCell), CoverCell.Key);

			View.ConstrainLayout(() =>
				_backButton.CenterX() == View.CenterX() &&
				_backButton.CenterY() == View.CenterY() && 
                _detailsButton.CenterX() == View.CenterX() &&
                _detailsButton.CenterY() == _backButton.Bottom() &&
				_collectionView.Top() == View.Top() &&
				_collectionView.Bottom() == View.Bottom() &&
				_collectionView.Left() == View.Left() &&
				_collectionView.Right() == View.Right());

            View.AddSubviews(_collectionView, _backButton, _detailsButton);
		}

        protected override void BindControls(CompositeDisposable disposables)
        {
            this.WhenAnyValue(x => x.ViewModel.CoverViewModels)
                .WhereNotNull()
                .Select(x => new ReactiveCollectionViewSource<ICoverViewModel>(_collectionView, x, CoverCell.Key))
                .BindTo(_collectionView, x => x.Source)
                .DisposeWith(disposables);
        }

        private UICollectionViewLayout CreateCollectionViewLayout()
        {
            var layout = new UICollectionViewFlowLayout();
            layout.ScrollDirection = UICollectionViewScrollDirection.Vertical;
            layout.ItemSize = new CoreGraphics.CGSize(170, 250);
            return layout;
        }
	}
}
