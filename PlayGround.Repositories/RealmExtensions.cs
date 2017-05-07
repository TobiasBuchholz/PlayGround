using System.Threading;

namespace Realms
{
	public static class RealmExtensions
	{
		public static Realm GetInstanceWithoutCapturingContext(RealmConfigurationBase config = null)
		{
			var context = SynchronizationContext.Current;
			SynchronizationContext.SetSynchronizationContext(null);

			Realm realm = null;
			try {
				realm = Realm.GetInstance(config);
			}
			finally {
				SynchronizationContext.SetSynchronizationContext(context);
			}
			return realm;
		}
	}
}
