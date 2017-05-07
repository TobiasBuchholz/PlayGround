using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using PlayGround.Contracts.Repositories;
using PlayGround.Contracts.Services.ServerApi;
using PlayGround.Models;
using Realms;

namespace PlayGround.Repositories
{
	public delegate Realm RealmFactory();

	public class CoversRepository : ICoversRepository
	{
		private readonly RealmFactory _realmFactory;
		private readonly IApiServiceFactory _apiServiceFactory;
		private readonly ISubject<IEnumerable<Cover>> _allSubject;

		public CoversRepository(
			RealmFactory realmFactory,
			IApiServiceFactory apiServiceFactory)
		{
			_realmFactory = realmFactory;
			_apiServiceFactory = apiServiceFactory;
			_allSubject = new Subject<IEnumerable<Cover>>();

			CreateCoversQueryable().SubscribeForNotifications((sender,_,__) => _allSubject.OnNext(sender));
		}

		private IOrderedQueryable<Cover> CreateCoversQueryable()
		{
			return _realmFactory()
				.All<Cover>()
				.OrderByDescending(x => x.PublishedAt);
		}

		public IObservable<IEnumerable<Cover>> GetCovers()
		{
			return _allSubject
				.AsObservable()
				.StartWith(CreateCoversQueryable());
		}

		public IObservable<Unit> UpdateCovers()
		{
			return _apiServiceFactory
				.CreateSimpleApiService<CoversContainer>("https://delivery.staging.pmx-cloud.com/v4/en/5754567386df/editions.json")
				.AuthorizedGet("Token c8596c725360d4ff0dc6")
				.Do(x => 
				{
					var realm = _realmFactory();
					realm.Write(() => realm.Add(x, update:true));
				})
				.ToSignal();
		}
	}
}
