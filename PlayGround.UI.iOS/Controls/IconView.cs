using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using FFImageLoading;
using FFImageLoading.Svg.Platform;
using ReactiveUI;
using UIKit;

namespace PlayGround.UI.iOS.Controls
{
    public class IconView : UIButton
    {
        public const int StateNormal = 0;
        public const int StateHighlighted = 1;

        private readonly CompositeDisposable _disposables;
        private string _icon;
        private nfloat _iconSize;
        private bool _isHighlighted;
        private UIColor[] _tintColors;

        public IconView()
        {
            _disposables = new CompositeDisposable();
        }

        public string Icon { 
            get { return _icon; }
            set {
                _icon = value;
                LoadIcon();
            }
        }

        private void LoadIcon()
        {
            ImageService
	            .Instance
                .LoadFile($"Icons/{_icon}.svg")
                .WithCustomDataResolver(new SvgDataResolver(0, (int) _iconSize, true))
	            .AsUIImageAsync()
	            .ToObservable()
	            .ObserveOn(RxApp.MainThreadScheduler)
	            .Select(x => x.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate))
                .SubscribeSafe(x => SetImage(x, UIControlState.Normal))
                .DisposeWith(_disposables);
        }

        public nfloat IconSize { 
            get { return _iconSize; } 
            set {
                _iconSize = value * UIScreen.MainScreen.Scale;
                LoadIcon();
            } 
        }

        public UIColor[] TintColors { 
            get { return _tintColors; } 
            set {
                _tintColors = value;
                ApplyTintColorsIfAllowed();
            } 
        }

        private void ApplyTintColorsIfAllowed()
        {
            if (TintColors?.Length > 1) {
                TintColor = Highlighted ? TintColors[StateHighlighted] : TintColors[StateNormal];
            }
        }

        public override bool Highlighted
        {
            get { return _isHighlighted; }
            set
            {
                _isHighlighted = value;
                ApplyTintColorsIfAllowed();
            }
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
