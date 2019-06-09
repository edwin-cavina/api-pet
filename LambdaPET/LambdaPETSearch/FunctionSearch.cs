using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Nest;
using Newtonsoft.Json;
using PET.Core;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LambdaPETSearch
{
	public class FunctionSearch
	{

		public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest req, ILambdaContext _lambdaContext)
		{
			try
			{
				if (req != null && !string.IsNullOrEmpty(req.Body))
				{
					return Search(JsonConvert.DeserializeObject<QueryModel>(req.Body));
				}
			}
			catch (Exception ex)
			{
				return new ReturnModel(false, new List<string> { ex.ToString() }).CreateResponse(HttpStatusCode.InternalServerError);
			}

			return new ReturnModel("Sem Filtros", false).CreateResponse(HttpStatusCode.BadRequest);
		}

		public APIGatewayProxyResponse Search(QueryModel _query)
		{
			ConnectionSettings connectionSettings = new ConnectionSettings(new Uri("https://search-es-east-dev-pet-bbzptvhnwezkoht4zvic6dqy7e.us-east-1.es.amazonaws.com"));
			ElasticClient elasticClient = new ElasticClient(connectionSettings);
			Console.WriteLine("query");
			Console.WriteLine(JsonConvert.SerializeObject(_query));

			ISearchResponse<PetModel> response = null;

			if (_query.PetType != null && _query.PetAge != null && _query.Color != null)
			{
				Console.WriteLine("1");
				response = elasticClient.Search<PetModel>(s => s.Index("user").Type("pet").Query(q => q.Bool(b => b.Must(
				m => m.Match(ma => ma.Field("petType").Query(_query.PetType.ToString())),
				m => m.Match(ma => ma.Field("petAge").Query(_query.PetAge.ToString())),
				m => m.Match(ma => ma.Field("color").Query(_query.Color.ToString()))
				).MustNot(m => m.Match(ma => ma.Field("idOwner").Query(_query.id)))
				)));
			}
			if ((_query.PetType != null && _query.PetAge != null && _query.Color == null))
			{
				Console.WriteLine("2");
				response = elasticClient.Search<PetModel>(s => s.Index("user").Type("pet").Query(q => q.Bool(b => b.Must(
				m => m.Match(ma => ma.Field("petType").Query(_query.PetType.ToString())),
				m => m.Match(ma => ma.Field("petAge").Query(_query.PetAge.ToString()))
				).MustNot(m => m.Match(ma => ma.Field("idOwner").Query(_query.id)))
				)));
			}
			if ((_query.PetType != null && _query.PetAge == null && _query.Color == null))
			{
				Console.WriteLine("3");
				response = elasticClient.Search<PetModel>(s => s.Index("user").Type("pet").Query(q => q.Bool(b => b.Must(
				m => m.Match(ma => ma.Field("petType").Query(_query.PetType.ToString()))
				).MustNot(m => m.Match(ma => ma.Field("idOwner").Query(_query.id)))
				)));
			}
			if ((_query.PetType == null && _query.PetAge != null && _query.Color != null))
			{
				Console.WriteLine("4");
				response = elasticClient.Search<PetModel>(s => s.Index("user").Type("pet").Query(q => q.Bool(b => b.Must(
				m => m.Match(ma => ma.Field("color").Query(_query.Color.ToString())),
				m => m.Match(ma => ma.Field("petAge").Query(_query.PetAge.ToString()))
				).MustNot(m => m.Match(ma => ma.Field("idOwner").Query(_query.id)))
				)));
			}
			if ((_query.PetType == null && _query.PetAge == null && _query.Color != null))
			{
				Console.WriteLine("5");
				response = elasticClient.Search<PetModel>(s => s.Index("user").Type("pet").Query(q => q.Bool(b => b.Must(
				m => m.Match(ma => ma.Field("color").Query(_query.Color.ToString()))
				).MustNot(m => m.Match(ma => ma.Field("idOwner").Query(_query.id)))
				)));
			}
			if ((_query.PetType != null && _query.PetAge == null && _query.Color != null))
			{
				Console.WriteLine("6");
				response = elasticClient.Search<PetModel>(s => s.Index("user").Type("pet").Query(q => q.Bool(b => b.Must(
				m => m.Match(ma => ma.Field("petType").Query(_query.PetType.ToString())),
				m => m.Match(ma => ma.Field("color").Query(_query.Color.ToString()))
				).MustNot(m => m.Match(ma => ma.Field("idOwner").Query(_query.id)))
				)));
			}
			if ((_query.PetType == null && _query.PetAge != null && _query.Color == null))
			{
				Console.WriteLine("7");
				response = elasticClient.Search<PetModel>(s => s.Index("user").Type("pet").Query(q => q.Bool(b => b.Must(
				m => m.Match(ma => ma.Field("petAge").Query(_query.PetAge.ToString()))
				).MustNot(m => m.Match(ma => ma.Field("idOwner").Query(_query.id)))
				)));
			}
			if ((_query.PetType == null && _query.PetAge == null && _query.Color == null))
			{
				Console.WriteLine("7");
				response = elasticClient.Search<PetModel>(s => s.Index("user").Type("pet").Query(q => q.Bool(b => b
				.MustNot(m => m.Match(ma => ma.Field("idOwner").Query(_query.id)))
				)));
			}

			if (response.Documents.Count != 0)
			{
				Console.WriteLine("RESULT");
				Console.WriteLine(JsonConvert.SerializeObject(response.Documents));
				return new ReturnModel(response.Documents, true).CreateResponse();
			}
			else
			{
				return new ReturnModel("Sem pets", false).CreateResponse();
			}
		}
	}
}
