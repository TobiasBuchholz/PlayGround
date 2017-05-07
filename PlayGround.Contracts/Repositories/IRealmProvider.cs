using Realms;
namespace PlayGround.Contracts.Repositories
{
	public interface IRealmProvider
	{
		Realm GetRealm();
	}
}
