using System;
using System.Reactive.Disposables;
using ReactiveUI;

namespace PlayGround.UI.iOS.Utility
{
    public abstract class CollectionViewCellBase<T> : ReactiveCollectionViewCell<T>
        where T : class
    {
        private readonly CompositeDisposable disposables;
        private bool didSetupConstraints;

        protected CollectionViewCellBase(IntPtr handle)
            : base(handle)
        {
            this.disposables = new CompositeDisposable();
            this.CreateView();
            this.CreateBindings();
            this.UpdateConstraints();
        }

        protected CompositeDisposable Disposables => this.disposables;

        public sealed override void UpdateConstraints()
        {
            base.UpdateConstraints();

            if (this.didSetupConstraints)
            {
                return;
            }

            this.UpdateConstraintsCore();
            this.didSetupConstraints = true;
        }

        public sealed override void LayoutSubviews()
        {
            base.LayoutSubviews();

            this.ContentView.SetNeedsLayout();
            this.ContentView.LayoutIfNeeded();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                this.disposables.Dispose();
            }
        }

        protected abstract void UpdateConstraintsCore();

        protected abstract void CreateView();

        protected abstract void CreateBindings();
    }
}
