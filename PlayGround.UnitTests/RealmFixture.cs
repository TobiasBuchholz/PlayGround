using System;
using System.Linq;
using System.Threading.Tasks;
using PlayGround.Models;
using Realms;
using Realms.Sync;
using Xunit;

namespace PlayGround.UnitTests
{
	public class RealmFixture
	{
		[Fact]
		public async Task can_login_and_logout_at_realm_mobile_platform()
		{
			var ip = "52.166.72.171";
			var credentials = Credentials.UsernamePassword("unittest@playground.com", "test", false);
			var authURL = new Uri($"http://{ip}:9080");
			var user = await User.LoginAsync(credentials, authURL);
			Assert.True(user.State == UserState.Active);

			user.LogOut();
			Assert.True(user.State == UserState.LoggedOut);
		}

		[Fact]
		public async Task can_add_recipe_to_realm()
		{
			var ip = "52.166.72.171";
			var credentials = Credentials.UsernamePassword("unittest@playground.com", "test", false);
			var authURL = new Uri($"http://{ip}:9080");
			var user = await User.LoginAsync(credentials, authURL);
			var config = new SyncConfiguration(user, new Uri($"realm://{ip}:9080/~/recipes"));
			var realm = RealmExtensions.GetInstanceWithoutCapturingContext(config);
			var recipesCount = realm.All<Recipe>().Count();
			realm.Write(() => realm.Add(new Recipe($"test_recipe_{recipesCount}")));
		}
	}
}
