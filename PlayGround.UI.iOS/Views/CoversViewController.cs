using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using PlayGround.Contracts.ViewModels;
using PlayGround.UI.iOS.Utility;
using ReactiveUI;
using UIKit;

namespace PlayGround.UI.iOS.Views
{
	public class CoversViewController : ViewControllerBase<ICoversViewModel>
	{
        private UIButton _backButton;
        private UIButton _forwardButton;
        private UICollectionView _collectionView;

		public CoversViewController()
		{
		}

        protected override void InitViews(CompositeDisposable disposables)
        {
            _backButton = ControlFactory
                .CreateButton()
                .DisposeWith(disposables);

            _backButton.SetTitle("Back", UIControlState.Normal);
            _backButton.TouchUpInside += (sender, e) => DismissViewController(true, null);

            _forwardButton = ControlFactory
                .CreateButton()
                .DisposeWith(disposables);

            _forwardButton.SetTitle("Forward", UIControlState.Normal);
            _forwardButton.TouchUpInside += (sender, e) => {
                var view = new DetailViewController();
                PresentViewController(view, true, null);
            };

            _collectionView = new UICollectionView(View.Frame, CreateCollectionViewLayout())
                .DisposeWith(disposables);
            _collectionView.RegisterClassForCell(typeof(CoverCell), CoverCell.Key);

            View.ConstrainLayout(() =>
				_backButton.CenterX() == View.CenterX() &&
				_backButton.CenterY() == View.CenterY() && 
				_forwardButton.CenterX() == View.CenterX() &&
				_forwardButton.Top() == _backButton.Bottom() && 
				_collectionView.Top() == View.Top() &&
				_collectionView.Bottom() == View.Bottom() &&
				_collectionView.Left() == View.Left() &&
				_collectionView.Right() == View.Right());

            View.AddSubviews(_collectionView, _backButton, _forwardButton);
        }

        protected override void BindGlobalControlsOnMainThread(CompositeDisposable disposables)
        {
            this.WhenAnyValue(x => x.ViewModel.CoverViewModels)
                .WhereNotNull()
                .Select(x => new ReactiveCollectionViewSource<ICoverViewModel>(_collectionView, x, CoverCell.Key).DisposeWith(disposables))
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
