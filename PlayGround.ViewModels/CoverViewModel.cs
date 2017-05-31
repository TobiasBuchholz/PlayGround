using PlayGround.Contracts.ViewModels;
using PlayGround.Models;
using ReactiveUI;
namespace PlayGround.ViewModels
{
	public class CoverViewModel : ReactiveObject, ICoverViewModel
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
	}
}
