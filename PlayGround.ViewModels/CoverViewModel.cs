using PlayGround.Contracts.ViewModels;
using PlayGround.Models;
using ReactiveUI;
namespace PlayGround.ViewModels
{
	public class CoverViewModel : ReactiveObject, ICoverViewModel
	{
		private readonly Cover _cover;

		public CoverViewModel(Cover cover)
		{
			_cover = cover;
		}

		public string Title => _cover.Title;
        public string ImageUrl => _cover.Links.ImageLink.Href;
	}
}
