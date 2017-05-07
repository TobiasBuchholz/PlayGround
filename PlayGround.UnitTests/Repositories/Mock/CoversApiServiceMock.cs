using System;
using PCLMock;
using PlayGround.Contracts.Services.ServerApi;
using PlayGround.Models;
using Refit;

namespace PlayGround.UnitTests.Repositories.Mock
{
	public class CoversApiServiceMock : MockBase<ISimpleApiService<CoversContainer>>, ISimpleApiService<CoversContainer>
	{
		public CoversApiServiceMock(MockBehavior behavior = MockBehavior.Strict)
			: base(behavior)
		{
			if(behavior == MockBehavior.Loose) {
				ConfigureLooseBehavior();
			}
		}

		private void ConfigureLooseBehavior()
		{
		}

		public IObservable<CoversContainer> AuthorizedGet([HeaderAttribute("Authorization")] string authToken)
		{
			return Apply(x => x.AuthorizedGet(authToken));
		}

		public IObservable<CoversContainer> Get()
		{
			return Apply(x => x.Get());
		}
	}
}
