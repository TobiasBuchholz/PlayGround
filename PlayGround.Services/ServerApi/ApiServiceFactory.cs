using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using PlayGround.Contracts.Services.ServerApi;
using Refit;

namespace PlayGround.Services.ServerApi
{
	public sealed class ApiServiceFactory : IApiServiceFactory
	{
		public const string baseUrl = "https://core.particles-staging.com/f/6a4b9323-348f-4517-a7c4-81c8b5579775";

		public ApiService CreateApiService<ApiService>()
		{
			return RestService.For<ApiService>(new HttpClient(new AuthenticatedHttpClientHandler()) { BaseAddress = new Uri(baseUrl) });
		}

		public ISimpleApiService<T> CreateSimpleApiService<T>(string url) where T : class
		{
			return RestService.For<ISimpleApiService<T>>(url); 
		}

		class AuthenticatedHttpClientHandler : HttpClientHandler
		{
			protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
			{
				request.Headers.Add("X-PreviewApp", "true");
				request.RequestUri = new Uri(request.RequestUri + ".json");
				return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
			}
		}
	}
}
