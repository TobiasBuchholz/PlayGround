using System.Collections.Generic;
using Newtonsoft.Json;

namespace PlayGround.Models
{
	public class CoversContainer
	{
		[JsonProperty("_embedded")]
		private readonly CoversContainerEmbedded _embedded;

		public IEnumerable<Cover> Covers { 
			get { 
				if(_embedded == null) {
					return new List<Cover>();
				} else {
					return _embedded.Covers;
				}
			}
		}
	}

	public class CoversContainerEmbedded
	{
		[JsonProperty("editions")]
		public IList<Cover> Covers { get; set; }
	}
}
