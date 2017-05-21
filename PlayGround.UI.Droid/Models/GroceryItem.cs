using System;
using Java.Util;
using PlayGround.Models;

namespace PlayGround.UI.Droid.Models
{
    public class GroceryItem : IGroceryItem
    {
        public GroceryItem()
        {
        }

        public GroceryItem(object mapAsObject)
        {
            var map = mapAsObject as HashMap;
            Name = map.Get("name").ToString();
            Amount = int.Parse(map.Get("amount").ToString());
        }

        public object ToDictionary()
        {
            var map = new HashMap();
            map.Put("name", Name);
            map.Put("amount", Amount);
            return map;
        }

        public override string ToString()
        {
            return string.Format("[GroceryItem: Name={0}, Amount={1}]", Name, Amount);
        }

        public string Name { get; set; }
        public int Amount { get; set; }
    }
}
