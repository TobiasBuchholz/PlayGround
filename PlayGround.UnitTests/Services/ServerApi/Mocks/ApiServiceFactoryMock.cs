using PCLMock;
using PlayGround.Contracts.Services.ServerApi;
namespace PlayGround.UnitTests.Services.ServerApi.Mocks
{
	public class ApiServiceFactoryMock : MockBase<IApiServiceFactory>, IApiServiceFactory
	{
		public ApiServiceFactoryMock(MockBehavior behavior = MockBehavior.Strict)
			: base(behavior)
		{
			if(behavior == MockBehavior.Loose) {
				ConfigureLooseBehavior();
			}
		}

		private void ConfigureLooseBehavior()
		{
		}

		public ApiService CreateApiService<ApiService>()
		{
			return Apply(x => x.CreateApiService<ApiService>());
		}

		public ISimpleApiService<T> CreateSimpleApiService<T>(string url) where T : class
		{
			return Apply(x => x.CreateSimpleApiService<T>(url));
		}
	}
}
