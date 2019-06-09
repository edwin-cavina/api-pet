using Newtonsoft.Json;
using PET.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LambdaPETSearch
{
	public class QueryModel
	{
		[JsonProperty(PropertyName = "pet_type", NullValueHandling = NullValueHandling.Ignore)]
		public int ?PetType { get; set; } = null;

		[JsonProperty(PropertyName = "color", NullValueHandling = NullValueHandling.Ignore)]
		public string Color { get; set; } = null;

		[JsonProperty(PropertyName = "pet_age", NullValueHandling = NullValueHandling.Ignore)]
		public int ?PetAge { get; set; } = null;

		[JsonProperty(PropertyName = "id", NullValueHandling = NullValueHandling.Ignore)]
		public string id { get; set; } = null;
	}
}
