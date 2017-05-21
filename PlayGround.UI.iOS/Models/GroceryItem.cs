using System;
using Foundation;
using PlayGround.Models;

namespace PlayGround.UI.iOS.Models
{
    public class GroceryItem : IGroceryItem
    {
        public GroceryItem()
        {
        }

        public GroceryItem(object dictionaryAsObject)
        {
            var dictionary = dictionaryAsObject as NSDictionary;
            Name = dictionary["name"].ToString();
            Amount = int.Parse(dictionary["amount"].ToString());
        }

        public object ToDictionary()
        {
            object[] keys = {"name", "amount"};
            object[] values = { Name, Amount };
            return NSDictionary.FromObjectsAndKeys(values, keys, keys.Length);
        }

        public override string ToString()
        {
            return string.Format("[GroceryItem: Name={0}, Amount={1}]", Name, Amount);
        }

        public string Name { get; set; }
        public int Amount { get; set; }
    }
}
