using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PlayGround.Contracts.Services.Web
{
	public interface IHttpClientService
	{
		Task<Stream> GetStreamAsync(string url, CancellationToken cancellationToken = default(CancellationToken));
		Task<byte[]> GetBytesAsync(string url, CancellationToken cancellationToken = default(CancellationToken));
	}
}
