using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Nest;
using Newtonsoft.Json;
using PET.Core;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LambdaPETCreateTransaction
{
    public class FunctionCreateTransaction
    {
		public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest req, ILambdaContext _lambdaContext)
		{
			try
			{
				Console.WriteLine("1");
				if (req != null && !string.IsNullOrEmpty(req.Body) && !string.IsNullOrEmpty(req.Path))
				{
					Console.WriteLine("2");
					switch (req.HttpMethod.ToLower())
					{
						case "post":
							Console.WriteLine("3");
							return Save(JsonConvert.DeserializeObject<TransactionModel>(req.Body));
						case "put":
							Console.WriteLine("4");
							return Update(JsonConvert.DeserializeObject<TransactionModel>(req.Body));
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("5");
				return new ReturnModel(false, new List<string> { ex.ToString() }).CreateResponse(HttpStatusCode.InternalServerError);
			}
			Console.WriteLine("6");
			return new ReturnModel(false, new List<string> { "Ocorreu um erro inesperado" }).CreateResponse(HttpStatusCode.InternalServerError);

		}

		private APIGatewayProxyResponse Save(TransactionModel request)
		{
			if (Validate(request))
			{
				Console.WriteLine("3-1");
				request.Transaction = StatusTransaction.PendingApproval;
				Console.WriteLine(JsonConvert.SerializeObject(request));
				ConnectionSettings connectionSettings = new ConnectionSettings(new Uri("https://search-es-east-dev-pet-bbzptvhnwezkoht4zvic6dqy7e.us-east-1.es.amazonaws.com"));
				ElasticClient elasticClient = new ElasticClient(connectionSettings);
				var res = elasticClient.Index(request, i => i.Index("transaction").Type("pet"));
				Console.WriteLine(JsonConvert.SerializeObject(res));

				return new ReturnModel(res.Id, true).CreateResponse();

			}
			else
				return new ReturnModel(false, new List<string> { "Preencher todos os campos" }).CreateResponse(HttpStatusCode.BadRequest);
		}

		private APIGatewayProxyResponse Update(TransactionModel request)
		{
			if (Validate(request))
			{
				Console.WriteLine("4-1");
				ConnectionSettings connectionSettings = new ConnectionSettings(new Uri("https://search-es-east-dev-pet-bbzptvhnwezkoht4zvic6dqy7e.us-east-1.es.amazonaws.com"));
				ElasticClient elasticClient = new ElasticClient(connectionSettings);
				var document = elasticClient.Get<TransactionModel>(request.Id, idx => idx.Index("transaction").Type("pet")).Source;
				request.IdAdopter = document.IdAdopter;
				request.IdOwner = document.IdOwner;
				request.IdPet = document.IdPet;
				var response = elasticClient.Index(request, i => i.Index("transaction").Type("pet"));
				return new ReturnModel("Atualizado com sucesso", true).CreateResponse();

			}
			else
				return new ReturnModel(false, new List<string> { "Preencher todos os campos" }).CreateResponse(HttpStatusCode.BadRequest);
		}

		private static bool Validate(TransactionModel request)
		{
			var result = new List<ValidationResult>();
			return Validator.TryValidateObject(request, new ValidationContext(request, null, null), result, true);
		}
	}
}
