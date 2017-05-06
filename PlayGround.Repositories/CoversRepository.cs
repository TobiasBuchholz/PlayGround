using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using PlayGround.Contracts.Repositories;
using PlayGround.Contracts.Services.ServerApi;
using PlayGround.Models;

namespace PlayGround.Repositories
{
	public class CoversRepository : ICoversRepository
	{
		private readonly IApiServiceFactory _apiServiceFactory;

		public CoversRepository(IApiServiceFactory apiServiceFactory)
		{
			_apiServiceFactory = apiServiceFactory;
		}

		public IObservable<IEnumerable<Cover>> GetCovers()
		{
			return _apiServiceFactory
				.CreateSimpleApiService<CoversContainer>("https://delivery.staging.pmx-cloud.com/v4/en/5754567386df/editions.json")
				.AuthorizedGet("Token c8596c725360d4ff0dc6")
				.Select(coversContainer => coversContainer.Covers)
				.CatchAndLogException();
		}
	}
}
