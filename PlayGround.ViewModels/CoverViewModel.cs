using System;
using System.Reactive.Disposables;
using PlayGround.Contracts.ViewModels;
using PlayGround.Models;
namespace PlayGround.ViewModels
{
    public class CoverViewModel : ViewModelBase, ICoverViewModel
	{
		private readonly string _title;
        private readonly string _imageUrl;

		public CoverViewModel(Cover cover)
		{
            _title = cover.Title;
            _imageUrl = cover.Links.ImageLink.Href;
		}

        public string Title => _title;
        public string ImageUrl => _imageUrl;

        protected override void InitLifeCycleAwareProperties(CompositeDisposable lifeCycleDisposable)
        {
        }
    }
}
