﻿using System;
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
	public class CoversRepository : ICoversRepository
	{
		private readonly IRealmProvider _realmProvider;
		private readonly IApiServiceFactory _apiServiceFactory;

		public CoversRepository(
			IRealmProvider realmProvider,
			IApiServiceFactory apiServiceFactory)
		{
			_realmProvider = realmProvider;
			_apiServiceFactory = apiServiceFactory;
		}

		private IOrderedQueryable<Cover> CreateCoversQueryable()
		{
			return _realmProvider.GetRealm()
				.All<Cover>()
				.OrderByDescending(x => x.PublishedAt);
		}

        public IObservable<IEnumerable<Cover>> GetCovers()
        {
			var subject = new Subject<IEnumerable<Cover>>();
            var covers = CreateCoversQueryable();
            covers.SubscribeForNotifications((sender,_,__) => subject.OnNext(sender));
            return subject
                .AsObservable()
                .StartWith(covers);
        }

		public IObservable<Unit> UpdateCovers()
		{
			return _apiServiceFactory
				.CreateSimpleApiService<CoversContainer>("https://delivery.staging.pmx-cloud.com/v4/en/5754567386df/editions.json")
				.AuthorizedGet("Token c8596c725360d4ff0dc6")
				.Do(x => 
				{
					var realm = _realmProvider.GetRealm();
					realm.Write(() => realm.Add(x, update:true));
				})
				.ToSignal();
		}
	}
}
