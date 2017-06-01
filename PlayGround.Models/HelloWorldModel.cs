using Realms;
namespace PlayGround.Models
{
	public class HelloWorldModel : RealmObject
	{
        public HelloWorldModel()
        {
        }

		public HelloWorldModel(long tick)
		{
			Name = "Hello world! " + tick;
		}

		[PrimaryKey]
		public long Id { get; set; }
		public string Name { get; set; }
	}
}
