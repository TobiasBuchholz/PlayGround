using PlayGround.Contracts.Repositories;
using PlayGround.Repositories;
using PlayGround.UnitTests.TestUtils;
using Realms;

namespace PlayGround.UnitTests.Repositories.Robots
{
	public sealed class CoversRepositoryRobot : BaseRobot<CoversRepositoryRobot, CoversRepositoryRobotResult>
	{
		private ICoversRepository _sut;

		public CoversRepositoryRobot()
		{
		}

		public override CoversRepositoryRobot Build()
		{
			base.Build();
			_sut = new CoversRepository(
				() => RealmExtensions.GetInstanceWithoutCapturingContext(),
				_apiServiceFactory);
			return this;
		}

		protected override CoversRepositoryRobotResult CreateResult()
		{
			return new CoversRepositoryRobotResult();
		}
	}

	public sealed class CoversRepositoryRobotResult : BaseRobotResult
	{
		
	}
}
