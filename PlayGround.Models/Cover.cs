using System;
using Newtonsoft.Json;
using Realms;
namespace PlayGround.Models
{
	public class Cover : RealmObject
	{
		[PrimaryKey]
		[JsonProperty("id")]
		public long Id { get; set; }

		[JsonProperty("token")]
		public string Token { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("version")]
		public string Version { get; set; }

		[JsonProperty("published_at")]
		public DateTimeOffset PublishedAt { get; set; }

		public string PublishedAtAsString {
			get { return PublishedAt.ToString("dd.MM.yyyy"); }
		}

		[JsonProperty("price")]
		public string Price { get; set; }

		[JsonProperty("currency")]
		public string Currency { get; set; }

		[JsonProperty("filesize")]
		public long FileSize { get; set; }
	}
}
