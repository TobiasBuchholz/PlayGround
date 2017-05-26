using System.Reactive.Disposables;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Support.V4.Content;
using Android.Util;
using Android.Views;
using FFImageLoading;
using FFImageLoading.Svg.Platform;
using FFImageLoading.Views;

namespace PlayGround.UI.Droid.Controls
{
    public class IconView : ImageViewAsync
    {
        private string _icon;
        private int _iconSize;

        public IconView(Context context)
            : base(context)
        {
            Initialize(context);
        }

        public IconView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            Initialize(context, attrs);
        }

        public IconView(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
            Initialize(context, attrs);
        }

        private void Initialize(Context context, IAttributeSet attrs = null)
        {
            InitAttributeSetIfAllowed(context, attrs);
            LoadIcon();
            Clickable = true;
        }

        private void InitAttributeSetIfAllowed(Context context, IAttributeSet attrs)
        {
            if (attrs != null)
            {
                InitAttributeSet(context, attrs);
            }
        }

        private void InitAttributeSet(Context context, IAttributeSet attrs) 
        {
            TypedArray typedArray = context.Theme.ObtainStyledAttributes(attrs, Resource.Styleable.IconView, 0, 0);
            try {
                _icon = typedArray.GetString(Resource.Styleable.IconView_iv_icon);
                _iconSize = typedArray.GetDimensionPixelSize(Resource.Styleable.IconView_iv_iconSize, 20);
            } finally {
                typedArray.Recycle();
            }
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
                .LoadCompiledResource($"{_icon}.svg")
                .WithCustomDataResolver(new SvgDataResolver(0, _iconSize, true))
                .Into(this);
        }

        public int IconSize { 
            get { return _iconSize; } 
            set {
                _iconSize = value;
                LoadIcon();
            } 
        }

        public ColorStateList TintColors {
            get { return base.ImageTintList; }
            set { base.ImageTintList = value; }
        }

        public Color TintColor {
            set { SetColorFilter(value); }
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if(e.Action == MotionEventActions.Up) {
                SetColorFilter(GetColorForState(-Android.Resource.Attribute.StatePressed));
            } else if(e.Action == MotionEventActions.Down) {
                SetColorFilter(GetColorForState(Android.Resource.Attribute.StatePressed));
            }
            return base.OnTouchEvent(e);
        }

        private Color GetColorForState(int state)
        {
            return new Color(ImageTintList.GetColorForState(new int[] { state }, Color.Black ));
        }
    }
}
