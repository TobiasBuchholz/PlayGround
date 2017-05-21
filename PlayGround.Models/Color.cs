using System;
using Newtonsoft.Json;

namespace PlayGround.Models
{
    public class Color
    {
        public Color()
        {
        }

        public override string ToString()
        {
            return string.Format("[Color: HexValue={0}]", HexValue);
        }

        [JsonProperty("hex_value")]
        public string HexValue { get; set; }
    }
}
