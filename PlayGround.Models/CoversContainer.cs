using System.Collections.Generic;
using Newtonsoft.Json;
using Realms;

namespace PlayGround.Models
{
	public class CoversContainer : RealmObject
	{
		[JsonProperty("_embedded")]
		public CoversContainerEmbedded Embedded { get; set; }

		[Ignored]
		public IList<Cover> Covers { 
			get { 
				if(Embedded == null) {
					return new List<Cover>();
				} else {
					return Embedded.Covers;
				}
			}
		}
	}

	public class CoversContainerEmbedded : RealmObject
	{
		[JsonProperty("editions")]
		public IList<Cover> Covers { get; }
	}
}
