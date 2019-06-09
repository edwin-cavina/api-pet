using System;
using System.Collections.Generic;
using System.Net;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using Nest;
using PET.Core;
using System.Linq;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LambdaPETGetUser
{
	public class FunctionGetUser
	{
		public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest req, ILambdaContext _lambdaContext)
		{
			try
			{
				if (req != null)
				{
					switch (req.HttpMethod.ToLower())
					{
						case "get":
							var id = req.QueryStringParameters["id"];
							return Get(id);
						case "post":
							return Login(JsonConvert.DeserializeObject<UserRequest>(req.Body));
					}
				}
			}
			catch (Exception ex)
			{
				return new ReturnModel(false, new List<string> { ex.ToString() }).CreateResponse(HttpStatusCode.InternalServerError);
			}
			return new ReturnModel(false, new List<string> { "Ocorreu um erro inesperado" }).CreateResponse(HttpStatusCode.InternalServerError);
		}

		public APIGatewayProxyResponse Get(string id)
		{
			ConnectionSettings connectionSettings = new ConnectionSettings(new Uri("https://search-es-east-dev-pet-bbzptvhnwezkoht4zvic6dqy7e.us-east-1.es.amazonaws.com"));
			ElasticClient elasticClient = new ElasticClient(connectionSettings);

			var response = elasticClient.Get<UserModel>(id, idx => idx.Index("pet").Type("user")).Source;
			Console.WriteLine(JsonConvert.SerializeObject(response));
			return new ReturnModel(response, true).CreateResponse();
		}

		public APIGatewayProxyResponse Login(UserRequest request) 
		{
			ConnectionSettings connectionSettings = new ConnectionSettings(new Uri("https://search-es-east-dev-pet-bbzptvhnwezkoht4zvic6dqy7e.us-east-1.es.amazonaws.com"));
			ElasticClient elasticClient = new ElasticClient(connectionSettings);

			var response = elasticClient.Search<UserModel>(s => s.Index("pet").Type("user").Query(q => q.Term(t => t.Field("email").Value(request.Email))));
			var user = (UserModel)response.Documents.FirstOrDefault();

			if(user.Password == request.Password && user != null)
			{
				return new ReturnModel(response.Documents.FirstOrDefault(), true).CreateResponse();
			}
			return new ReturnModel("Email ou senha incorretos!", false).CreateResponse();
		}
	}
}
