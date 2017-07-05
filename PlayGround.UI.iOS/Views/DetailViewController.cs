using System;
using CoreGraphics;
using UIKit;

namespace PlayGround.UI.iOS.Views
{
    public class DetailViewController : UIViewController
    {
        public DetailViewController()
        {
        }

        public override void LoadView()
        {
            base.LoadView();
            var backButton = new UIButton(new CGRect(100, 100, 100, 100));
            backButton.SetTitle("Back", UIControlState.Normal);
            backButton.TouchUpInside += (sender, e) => DismissViewController(true, null);
            View.AddSubview(backButton);

            View.BackgroundColor = UIColor.Green;
        }
    }
}
