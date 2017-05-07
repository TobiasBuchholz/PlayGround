using PlayGround.Contracts.Repositories;
using Realms;

namespace PlayGround.UnitTests.Repositories
{
	public class TestRealmProvider : IRealmProvider
	{
		public Realm GetRealm()
		{
			return RealmExtensions.GetInstanceWithoutCapturingContext();
		}
	}
}
