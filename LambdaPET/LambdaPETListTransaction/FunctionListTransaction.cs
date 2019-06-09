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

namespace LambdaPETListTransaction
{
    public class FunctionListTransaction
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

		public APIGatewayProxyResponse Get(string id)
		{
			ConnectionSettings connectionSettings = new ConnectionSettings(new Uri("https://search-es-east-dev-pet-bbzptvhnwezkoht4zvic6dqy7e.us-east-1.es.amazonaws.com"));
			ElasticClient elasticClient = new ElasticClient(connectionSettings);

			var responseOwner = elasticClient.Search<TransactionModel>(s => s.Index("transaction").Type("pet").Query(q => q.Bool(b => b.Must(m => m.Match(ma => ma.Field("idOwner").Query(id))))));
			Console.WriteLine(JsonConvert.SerializeObject(responseOwner));

			var responseAdopter = elasticClient.Search<TransactionModel>(s => s.Index("transaction").Type("pet").Query(q => q.Bool(b => b.Must(m => m.Match(ma => ma.Field("idAdopter").Query(id))))));
			Console.WriteLine(JsonConvert.SerializeObject(responseAdopter));

			ListTransacton response = new ListTransacton { ListOwner = (List<TransactionModel>)responseOwner.Documents, ListAdopter = (List<TransactionModel>)responseAdopter.Documents };

			return new ReturnModel(response,true).CreateResponse();
		}
	}
}
