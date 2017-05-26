using System;
using System.Reactive.Disposables;
using FFImageLoading;
using Foundation;
using PlayGround.Contracts.ViewModels;
using PlayGround.UI.iOS.Utility;
using ReactiveUI;
using UIKit;

namespace PlayGround.UI.iOS.Views
{
    public sealed class CoverCell : CollectionViewCellBase<ICoverViewModel>
    {
        public static readonly NSString Key = new NSString("key");

        private UILabel _titleLabel;
        private UIImageView _imageView;

        public CoverCell(IntPtr handle)
            : base(handle)
        {
        }

        protected override void CreateView()
        {
            _titleLabel = ControlFactory.CreateLabel().DisposeWith(Disposables);
            _imageView = ControlFactory.CreateImage().DisposeWith(Disposables);
            ContentView.AddSubviews(_imageView, _titleLabel);
        }

        protected override void UpdateConstraintsCore()
        {
            ContentView.ConstrainLayout(() =>
				_titleLabel.CenterX() == ContentView.CenterX() &&
                _titleLabel.CenterY() == ContentView.CenterY() && 
                _imageView.Top() == ContentView.Top() &&
                _imageView.Bottom() == ContentView.Bottom() &&
                _imageView.Left() == ContentView.Left() &&
                _imageView.Right() == ContentView.Right());
        }

        protected override void CreateBindings()
        {
            this.WhenAnyValue(x => x.ViewModel.Title)
                .BindTo(_titleLabel, x => x.Text)
                .DisposeWith(Disposables);

            this.WhenAnyValue(x => x.ViewModel.ImageUrl)
                .SubscribeSafe(x => 
            {
                ImageService
                    .Instance
                    .LoadUrl(x)
                    .Into(_imageView);
            }).DisposeWith(Disposables);
        }
    }
}
