namespace PlayGround.Contracts.Services.ServerApi
{
	public interface IApiServiceFactory
	{
		ApiService CreateApiService<ApiService>();
		ISimpleApiService<T> CreateSimpleApiService<T>(string url) where T : class;
	}
}
