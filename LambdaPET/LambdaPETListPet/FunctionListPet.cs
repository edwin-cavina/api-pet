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

namespace LambdaPETListPet
{
    public class FunctionListPet
    {
		public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest req, ILambdaContext _lambdaContext)
		{
			try
			{
				var id = req.QueryStringParameters["idOwner"];
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

			var response = elasticClient.Search<PetModel>(s => s.Index("user").Type("pet").Query(q => q.Bool(b => b.Must(m => m.Match(ma => ma.Field("idOwner").Query(id))))));
			Console.WriteLine(JsonConvert.SerializeObject(response));

			if (response.Documents.Count != 0)
			{
				return new ReturnModel(response.Documents, true).CreateResponse();
			}
			else 
			{
				return new ReturnModel("Este usuário não possui PET cadastrado", false).CreateResponse();
			}
		}
	}
}
