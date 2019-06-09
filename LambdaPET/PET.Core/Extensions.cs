using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;

namespace PET.Core
{
	public static class Extensions
	{
		public static APIGatewayProxyResponse CreateResponse(this object _object, HttpStatusCode _httpStatusCode = HttpStatusCode.OK)
		{
			return new APIGatewayProxyResponse()
			{
				StatusCode = (int)_httpStatusCode,
				Headers = new Dictionary<string, string> { { "Access-Control-Allow-Origin", "*" }, { "Access-Control-Allow-Headers", "*" } },
				Body = JsonConvert.SerializeObject(_object)
			};
		}
	}
}
