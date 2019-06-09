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

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LambdaPETGetTransaction
{
	public class FunctionGetTransaction
	{
		public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest req, ILambdaContext _lambdaContext)
		{
			try
			{
				Console.WriteLine(JsonConvert.SerializeObject(req));
				var id = req.QueryStringParameters["id"];
				Console.WriteLine(id);
				return Get(id);
			}
			catch (Exception ex)
			{
				return new ReturnModel(false, new List<string> { ex.ToString() }).CreateResponse(HttpStatusCode.InternalServerError);
			}
		}

		public APIGatewayProxyResponse Get(string id)
		{
			ConnectionSettings connectionSettings = new ConnectionSettings(new Uri("https://search-es-east-dev-pet-bbzptvhnwezkoht4zvic6dqy7e.us-east-1.es.amazonaws.com"));
			ElasticClient elasticClient = new ElasticClient(connectionSettings);

			var response = elasticClient.Get<TransactionModel>(id, idx => idx.Index("transaction").Type("pet")).Source;
			Console.WriteLine(JsonConvert.SerializeObject(response));
			return new ReturnModel(response, true).CreateResponse();
		}
	}
}
