using System.IO;
using System.Threading;
using System.Threading.Tasks;
using PlayGround.Contracts.Services.Web;

namespace PlayGround.Services.Web
{
	public class HttpClientService : IHttpClientService
	{
		public async Task<byte[]> GetBytesAsync(string url, CancellationToken cancellationToken = default(CancellationToken))
		{
			using(var httpClient = new System.Net.Http.HttpClient()) 
			{
				var response = await httpClient.GetAsync(url, cancellationToken);
				return await response.Content.ReadAsByteArrayAsync();	
			}
		}

		public async Task<Stream> GetStreamAsync(string url, CancellationToken cancellationToken = default(CancellationToken))
		{
			using(var httpClient = new System.Net.Http.HttpClient()) 
			{
				var response = await httpClient.GetAsync(url, cancellationToken);
				return await response.Content.ReadAsStreamAsync();
			}
		}
	}
}
