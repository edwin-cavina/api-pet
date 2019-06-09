using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Model;
using Nest;
using Newtonsoft.Json;
using PET.Core;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LambdaPETUploadPhotos
{
	public class FunctionUploadPhotos
	{
		public AmazonS3Client Client { get; set; } = null;

		public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest req, ILambdaContext _lambdaContext)
		{
			try
			{
				if (req != null && !string.IsNullOrEmpty(req.Body))
				{
					return Upload(JsonConvert.DeserializeObject<UploadModel>(req.Body));
				}
			}
			catch (Exception ex)
			{
				return new ReturnModel(false, new List<string> { ex.ToString() }).CreateResponse(HttpStatusCode.InternalServerError);
			}

			return new ReturnModel(false, new List<string> { "Ocorreu um erro inesperado" }).CreateResponse(HttpStatusCode.InternalServerError);
		}

		private APIGatewayProxyResponse Upload(UploadModel request)
		{
			ConnectionSettings connectionSettings = new ConnectionSettings(new Uri("https://search-es-east-dev-pet-bbzptvhnwezkoht4zvic6dqy7e.us-east-1.es.amazonaws.com"));
			ElasticClient elasticClient = new ElasticClient(connectionSettings);
			var response = elasticClient.Get<UserModel>(request.Id , idx => idx.Index("pet").Type("user")).Source;

			Client = new AmazonS3Client(RegionEndpoint.USEast1);

			if (response != null)
			{
				SaveS3(request, "pet-user-dev/");
			}
			else 
			{
				SaveS3(request, "user-pet-dev/");
			}

			return new ReturnModel("Salvo com sucesso", true).CreateResponse(HttpStatusCode.OK);
		}

		public void SaveS3(UploadModel request, string bucketName) 
		{

			var decodedImage = Convert.FromBase64String(request.ImgBase64);	
			Client.PutObjectAsync(new PutObjectRequest()
			{
				BucketName = bucketName + request.Id,
				Key = "1.jpg",
				InputStream = new MemoryStream(decodedImage)
			}).Wait();
		}
	}
}
