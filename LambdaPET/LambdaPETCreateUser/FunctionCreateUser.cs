using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using Nest;
using PET.Core;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LambdaPETCreateUser
{
	public class FunctionCreateUser
	{
		public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest req, ILambdaContext _lambdaContext)
		{
			try
			{
				Console.WriteLine("1");
				if (req != null && !string.IsNullOrEmpty(req.Body) && !string.IsNullOrEmpty(req.Path))
				{
					Console.WriteLine("2");
					return Save(JsonConvert.DeserializeObject<UserModel>(req.Body));
				}
			}
			catch(Exception ex)
			{
				Console.WriteLine("5");
				return new ReturnModel(false, new List<string> { ex.ToString() }).CreateResponse(HttpStatusCode.InternalServerError);
			}
			Console.WriteLine("6");
			return new ReturnModel(false, new List<string> { "Ocorreu um erro inesperado" }).CreateResponse(HttpStatusCode.InternalServerError);
		}

		private APIGatewayProxyResponse Save(UserModel request)
		{
			Console.WriteLine("3");
			if (Validate(request))
			{
				Console.WriteLine("4");
				ConnectionSettings connectionSettings = new ConnectionSettings(new Uri("https://search-es-east-dev-pet-bbzptvhnwezkoht4zvic6dqy7e.us-east-1.es.amazonaws.com"));
				ElasticClient elasticClient = new ElasticClient(connectionSettings);
				var res = elasticClient.Index(request, i => i.Index("pet").Type("user"));

				request.Id = res.Id;

				var response = elasticClient.Index(request, i => i.Index("pet").Type("user"));

				return new ReturnModel(res.Id, true).CreateResponse();

			}
			else
				return new ReturnModel(false, new List<string> { "Preencher todos os campos" }).CreateResponse(HttpStatusCode.BadRequest);
		}

		private static bool Validate(UserModel request)
		{
			var result = new List<ValidationResult>();
			return Validator.TryValidateObject(request, new ValidationContext(request, null, null), result, true);
		}
	}
}
