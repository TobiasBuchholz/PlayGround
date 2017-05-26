using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using FFImageLoading;
using Genesis.Logging;
using PlayGround.Contracts.ViewModels;
using PlayGround.UI.iOS.Utility;
using ReactiveUI;
using UIKit;

namespace PlayGround.UI.iOS.Views
{
	public class CoversViewController : ViewControllerBase<ICoversViewModel>
	{
        private UIButton _backButton;
        private UICollectionView _collectionView;

		public CoversViewController()
		{
			View.BackgroundColor = UIColor.Green;

			var logger = LoggerService.GetLogger(GetType());

			this.WhenActivated(disposables => {
				using (logger.Perf("Activation")) 
				{
                    this.WhenAnyValue(x => x.ViewModel.CoverViewModels)
                        .Select(x => x == null ? null : new ReactiveCollectionViewSource<ICoverViewModel>(_collectionView, x, CoverCell.Key))
                        .BindTo(_collectionView, x => x.Source)
                        .DisposeWith(Disposables);
				}
			});
		}

		public override void LoadView()
		{
			base.LoadView();

			_backButton = ControlFactory
				.CreateButton()
				.DisposeWith(Disposables);

			_backButton.SetTitle("Back", UIControlState.Normal);
			_backButton.TouchUpInside += (sender, e) => DismissViewController(true, null);

            _collectionView = new UICollectionView(View.Frame, CreateCollectionViewLayout());
            _collectionView.RegisterClassForCell(typeof(CoverCell), CoverCell.Key);

			View.ConstrainLayout(() =>
				_backButton.CenterX() == View.CenterX() &&
				_backButton.CenterY() == View.CenterY() && 
				_collectionView.Top() == View.Top() &&
				_collectionView.Bottom() == View.Bottom() &&
				_collectionView.Left() == View.Left() &&
				_collectionView.Right() == View.Right());

            View.AddSubviews(_collectionView, _backButton);
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
