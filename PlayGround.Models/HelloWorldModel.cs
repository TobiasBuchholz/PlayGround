using Realms;
namespace PlayGround.Models
{
	public class HelloWorldModel : RealmObject
	{
		public HelloWorldModel()
		{
			Name = "Hello world!";
		}

		[PrimaryKey]
		public long Id { get; set; }
		public string Name { get; set; }
	}
}
