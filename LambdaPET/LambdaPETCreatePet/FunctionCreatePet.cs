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

namespace LambdaPETCreatePet
{
	public class FunctionCreatePet
	{
		public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest req, ILambdaContext _lambdaContext)
		{
			try
			{
				if (req != null && !string.IsNullOrEmpty(req.Body) && !string.IsNullOrEmpty(req.Path))
				{
					switch (req.HttpMethod.ToLower())
					{
						case "post":
							return Save(JsonConvert.DeserializeObject<PetModel>(req.Body));
						case "put":
							return Update(JsonConvert.DeserializeObject<PetModel>(req.Body));
					}
				}
			}
			catch (Exception ex)
			{
				return new ReturnModel(false, new List<string> { ex.ToString() }).CreateResponse(HttpStatusCode.InternalServerError);
			}

			return new ReturnModel(false, new List<string> { "Ocorreu um erro inesperado" }).CreateResponse(HttpStatusCode.InternalServerError);
		}

		private APIGatewayProxyResponse Save(PetModel request)
		{
			if (Validate(request))
			{
				ConnectionSettings connectionSettings = new ConnectionSettings(new Uri("https://search-es-east-dev-pet-bbzptvhnwezkoht4zvic6dqy7e.us-east-1.es.amazonaws.com"));
				ElasticClient elasticClient = new ElasticClient(connectionSettings);
				var res = elasticClient.Index(request, i => i.Index("user").Type("pet"));

				request.Id = res.Id;

				var response = elasticClient.Index(request, i => i.Index("user").Type("pet"));

				return new ReturnModel(res.Id, true).CreateResponse();

			}
			else
				return new ReturnModel(false, new List<string> { "Preencher todos os campos" }).CreateResponse(HttpStatusCode.BadRequest);
		}

		private APIGatewayProxyResponse Update(PetModel request)
		{
			if (Validate(request))
			{
				ConnectionSettings connectionSettings = new ConnectionSettings(new Uri("https://search-es-east-dev-pet-bbzptvhnwezkoht4zvic6dqy7e.us-east-1.es.amazonaws.com"));
				ElasticClient elasticClient = new ElasticClient(connectionSettings);
				var res = elasticClient.Index(request, i => i.Index("user").Type("pet"));

				return new ReturnModel(res.Id, true).CreateResponse();

			}
			else
				return new ReturnModel(false, new List<string> { "Preencher todos os campos" }).CreateResponse(HttpStatusCode.BadRequest);
		}

		private static bool Validate(PetModel request)
		{
			var result = new List<ValidationResult>();
			return Validator.TryValidateObject(request, new ValidationContext(request, null, null), result, true);
		}
	}
}
