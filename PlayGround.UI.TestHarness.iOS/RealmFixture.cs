using Realms;
using Xunit;

namespace PlayGround.UI.TestHarness.iOS
{
	public class RealmFixture
	{
		[Fact]
		public void should_pass()
		{
			var realm = Realm.GetInstance();
			var transaction = realm.BeginWrite();
			var testModel = new TestModel();
			testModel.Id = 1337;
			testModel.Name = "Foo";
			transaction.Commit();
		}
	}

	public class TestModel : RealmObject
	{
		[PrimaryKey]
		public int Id { get; set; }

		public string Name { get; set; }
	}
}
