using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Nest;
using Newtonsoft.Json;
using PET.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LambdaPETGetPhotos
{
	public class FunctionGetPhotos
	{
		public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest req, ILambdaContext _lambdaContext)
		{
			try
			{
				var id = req.QueryStringParameters["id"];
				return Get(id);
			}
			catch (Exception ex)
			{
				return new ReturnModel(false, new List<string> { ex.ToString() }).CreateResponse(HttpStatusCode.InternalServerError);
			}
		}

		private APIGatewayProxyResponse Get(string id)
		{
			string url;
			ConnectionSettings connectionSettings = new ConnectionSettings(new Uri("https://search-es-east-dev-pet-bbzptvhnwezkoht4zvic6dqy7e.us-east-1.es.amazonaws.com"));
			ElasticClient elasticClient = new ElasticClient(connectionSettings);
			var response = elasticClient.Get<UserModel>(id, idx => idx.Index("pet").Type("user")).Source;

			if (response != null)
			{
				url = "https://pet-user-dev.s3.amazonaws.com/" + id + "/1.jpg";
			}
			else
			{
				url = "https://user-pet-dev.s3.amazonaws.com/" + id + "/1.jpg";
			}

			return new ReturnModel(url, true).CreateResponse();
		}
	}
}