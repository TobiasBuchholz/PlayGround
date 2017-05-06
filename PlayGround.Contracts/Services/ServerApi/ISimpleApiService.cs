using System;
using Refit;

namespace PlayGround.Contracts.Services.ServerApi
{
	public interface ISimpleApiService<T> where T : class
	{
		[Get("")]
		IObservable<T> Get();

		[Get("")]
		IObservable<T> AuthorizedGet([Header("Authorization")] string authToken);
	}
}
