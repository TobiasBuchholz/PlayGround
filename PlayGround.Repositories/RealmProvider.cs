using PlayGround.Contracts.Repositories;
using Realms;

namespace PlayGround.Repositories
{
	public class RealmProvider : IRealmProvider
	{
		public Realm GetRealm()
		{
			return Realm.GetInstance();
		}
	}
}
