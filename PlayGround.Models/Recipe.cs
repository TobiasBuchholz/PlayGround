using Realms;
namespace PlayGround.Models
{
	public class Recipe : RealmObject
	{
		public Recipe()
		{
			// empty constructor for realm
		}

		public Recipe(string name)
		{
			Name = name;
		}

		public string Name { get; set; }
	}
}
