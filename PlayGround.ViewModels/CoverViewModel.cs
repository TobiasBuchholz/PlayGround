using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using PlayGround.Contracts.Services.Web;
using PlayGround.Contracts.ViewModels;
using PlayGround.Models;
using ReactiveUI;
using Splat;

namespace PlayGround.ViewModels
{
	public class CoverViewModel : ReactiveObject, ICoverViewModel
	{
		private readonly Cover _cover;
		private readonly IHttpClientService _httpClientService;
		private readonly ObservableAsPropertyHelper<IBitmap> _image;

		public CoverViewModel(
			Cover cover,
			IHttpClientService httpClientService)
		{
			_cover = cover;
			_httpClientService = httpClientService;

			_httpClientService
				.GetStreamAsync(_cover.Links.ImageLink.Href)
				.ToObservable()
				.SelectMany(stream => BitmapLoader.Current.Load(stream, null, null).ToObservable())
				.ToProperty(this, x => x.Image, out _image);
		}

		public string Title => _cover.Title;
		public IBitmap Image => _image.Value;
	}
}
