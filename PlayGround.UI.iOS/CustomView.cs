using System;
using System.ComponentModel;
using Foundation;
using UIKit;

namespace PlayGround.UI.iOS
{
	[DesignTimeVisible(true)]  // This makes it visible in the Custom Controls panel in the iOS Designer.  The code-behind file already has the Register attribute 
	public partial class CustomView : UIView, IComponent  // IComponent is necessary to check DesignMode
	{
		public CustomView(IntPtr handle) : base (handle)
		{
		}

		#region IComponent implementation

		public ISite Site { get; set; }
		public event EventHandler Disposed;

		#endregion

		// This is an example of exposing a custom property that can be set via the iOS Designer
		[Export("Name"), Browsable(true)]
		public string Name { get; set; }

		public override void AwakeFromNib()
		{
			base.AwakeFromNib();

			if ((Site != null) && Site.DesignMode)
			{
				// Bundle resources aren't available in DesignMode
				return;
			}

			NSBundle.MainBundle.LoadNib("CustomView", this, null);

			// At this point all of the code-behind properties should be set, specifically rootView which must be added as a subview of this view

			this.AddSubview(this.RootView);

			this.NameLabel.Text = Name;  // ...and here's how you can use custom properties to set values on the code-behind properties
		}
	}
}