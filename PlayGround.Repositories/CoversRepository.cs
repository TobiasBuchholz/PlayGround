﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using PlayGround.Contracts.Repositories;
using PlayGround.Contracts.Services.ServerApi;
using PlayGround.Models;
using ReactiveUI;
using Realms;

namespace PlayGround.Repositories
{
	public class CoversRepository : ICoversRepository
	{
		private readonly IRealmProvider _realmProvider;
		private readonly IApiServiceFactory _apiServiceFactory;
        private readonly ISubject<IEnumerable<ThreadSafeReference.Object<Cover>>> _allCovers;

		public CoversRepository(
			IRealmProvider realmProvider,
			IApiServiceFactory apiServiceFactory)
		{
			_realmProvider = realmProvider;
			_apiServiceFactory = apiServiceFactory;
            _allCovers = new Subject<IEnumerable<ThreadSafeReference.Object<Cover>>>();

            CreateCoversQueryable()
                .SubscribeForNotifications((sender,_,__) => _allCovers.OnNext(sender.Select(x => ThreadSafeReference.Create(x)).ToList()));
		}

		private IOrderedQueryable<Cover> CreateCoversQueryable()
		{
			return _realmProvider
                .GetRealm()
				.All<Cover>()
				.OrderByDescending(x => x.PublishedAt);
		}

        public IObservable<IEnumerable<Cover>> GetCovers()
        {
            return _allCovers
                .AsObservable()
                .ObserveOn(RxApp.TaskpoolScheduler)
                .Select(references => references.Select(x => _realmProvider.GetRealm().ResolveReference(x)))
                .StartWith(CreateCoversQueryable());

            // the following is actually a bit more straight forward implementation, 
            // but does not work since SubscribeForNotifications won't get invoked 
            // constantly for some reason

			//var subject = new Subject<IEnumerable<Cover>>();
            //var covers = CreateCoversQueryable();
            //var token = covers.SubscribeForNotifications((sender,_,__) => {
            //    subject.OnNext(sender);
            //    System.Diagnostics.Debug.WriteLine("FU");
            //});

            //return subject
                //.AsObservable()
                //.StartWith(covers)
                //.Finally(token.Dispose);
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
