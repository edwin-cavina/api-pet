using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LambdaPETGetMessage
{
	class MessageModel
	{
		[JsonProperty(PropertyName = "message")]
		public string Message { get; set; } = null;

		[JsonProperty(PropertyName = "show_option")]
		public int Option { get; set; }
	}
}
