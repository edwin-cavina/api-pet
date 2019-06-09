using System;
using System.Collections.Generic;
using System.Net;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using Nest;
using PET.Core;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LambdaPETGetPet
{
	//Será que se eu chamar ela pra sair, ela aceita?
	public class FunctionGetPet
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

			var response = elasticClient.Get<PetModel>(id, idx => idx.Index("user").Type("pet")).Source;
			Console.WriteLine(JsonConvert.SerializeObject(response));

			return new ReturnModel(response, true).CreateResponse();

		}
	}
}
