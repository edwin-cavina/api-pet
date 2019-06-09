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

namespace LambdaPETGetMessage
{
    public class FunctionGetMessage
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

			var response = elasticClient.Search<TransactionModel>(s => s.Index("transaction").Type("pet").Query(q => q.Bool(b => b.Must(m => m.Match(ma => ma.Field("idOwner").Query(id))))));

			if (response.Documents.Count != 0 && response.Documents.First().Transaction == StatusTransaction.PendingApproval)
			{
				var person = elasticClient.Get<UserModel>(response.Documents.FirstOrDefault().IdAdopter, idx => idx.Index("pet").Type("user")).Source;
				return new ReturnModel(new MessageModel { Message = person.Name + " quer adotar o seu PET, aprovação pendente!", Option = 1 }, true).CreateResponse();
			}
			else {
				response = elasticClient.Search<TransactionModel>(s => s.Index("transaction").Type("pet").Query(q => q.Bool(b => b.Must(m => m.Match(ma => ma.Field("idAdopter").Query(id))))));
				if (response.Documents.Count != 0 && response.Documents.First().Transaction == StatusTransaction.Chatting)
				{
					var person = elasticClient.Get<UserModel>(response.Documents.FirstOrDefault().IdOwner, idx => idx.Index("pet").Type("user")).Source;
					return new ReturnModel(new MessageModel { Message = person.Name + ", dono do PET que você quer adotar aceitou o seu pedido, converse com ele agora!", Option = 2 }, true).CreateResponse();
				}
			}

			Console.WriteLine(JsonConvert.SerializeObject(response));

			return new ReturnModel(new MessageModel { Message = "Nenhuma atualização", Option = 0 }, true).CreateResponse();

		}
	}
}
