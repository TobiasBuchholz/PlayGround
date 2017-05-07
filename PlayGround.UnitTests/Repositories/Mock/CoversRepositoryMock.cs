using System;
using System.Collections.Generic;
using System.Reactive;
using PCLMock;
using PlayGround.Contracts.Repositories;
using PlayGround.Models;

namespace PlayGround.UnitTests.Repositories.Mock
{
	public class CoversRepositoryMock : MockBase<ICoversRepository>, ICoversRepository
	{
		public CoversRepositoryMock(MockBehavior behavior = MockBehavior.Strict)
			: base(behavior)
		{
			if(behavior == MockBehavior.Loose) {
				ConfigureLooseBehavior();
			}
		}

		private void ConfigureLooseBehavior()
		{
		}

		public IObservable<IEnumerable<Cover>> GetCovers()
		{
			return Apply(x => x.GetCovers());
		}

		public IObservable<Unit> UpdateCovers()
		{
			return Apply(x => x.UpdateCovers());
		}
	}
}
