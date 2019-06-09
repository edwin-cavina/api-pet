using System;
using System.Collections.Generic;
using Xunit;
using Amazon.Lambda.TestUtilities;
using LambdaPETCreateUser;
using LambdaPETCreatePet;
using PET.Core;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using LambdaPETGetPet;
using LambdaPETGetUser;
using LambdaPETListPet;
using LambdaPETSearch;
using LambdaPETUploadPhotos;
using LambdaPETCreateTransaction;
using LambdaPETGetMessage;
using LambdaPETListTransaction;

namespace LambdaPET.Tests
{
    public class FunctionTest
    {
		[Fact]
		public void CreateUser()
		{
			var function = new FunctionCreateUser();
			var context = new TestLambdaContext();
			var requestUser = new UserModel
			{
				Email = "edwin22@teste.com.br",
				Password = "teste123",
				Name = "Edwin Teste",
				DateBirth = DateTime.Now,
				Client = ClientType.Adopter,
				Person = PersonType.PF,
				DDDPhone = 11,
				Phone = 988887777,
				CEP = "01327000",
				Address = "Rua Treze de Maior",
				AddressComplement = "apto 81",
				City = "São Paulo",
				State = "SP",
				Cpf = "30030030030",
				Cnpj = "",
				Rg = "303033033"
			};
			var apiGateway = new APIGatewayProxyRequest
			{
				HttpMethod = "POST",
				Path = "create-user",
				Body = JsonConvert.SerializeObject(requestUser),
				Headers = new Dictionary<string, string>()
				{
					{ "Content-Type", "application/json" }
				}
			};
			var _return = function.FunctionHandler(apiGateway, context);
			Assert.Equal(200, _return.StatusCode);
		}

		[Fact]
		public void GetUser()
		{
			var function = new FunctionGetUser();
			var context = new TestLambdaContext();
			var request = new PetRequest
			{
				id = "LAZzUWoBfKkxXf3ErZ77"
			};
			var apiGateway = new APIGatewayProxyRequest
			{
				HttpMethod = "GET",
				Path = "get-user",
				Resource = "/dev/",
				Body = JsonConvert.SerializeObject(request),
				Headers = new Dictionary<string, string>()
				{
					{ "Content-Type", "application/json" }
				}
			};
			var _return = function.FunctionHandler(apiGateway, context);
			Assert.Equal(200, _return.StatusCode);
		}

		[Fact]
		public void Login()
		{
			var function = new FunctionGetUser();
			var context = new TestLambdaContext();
			var request = new UserRequest
			{
				Email = "cassiano@gmail.com",
				Password = "teste"
			};
			var apiGateway = new APIGatewayProxyRequest
			{
				HttpMethod = "POST",
				Path = "get-user",
				Resource = "/dev/",
				Body = JsonConvert.SerializeObject(request),
				Headers = new Dictionary<string, string>()
				{
					{ "Content-Type", "application/json" }
				}
			};
			var _return = function.FunctionHandler(apiGateway, context);
			Assert.Equal(200, _return.StatusCode);
		}

		[Fact]
		public void CreatePet()
		{
			var function = new FunctionCreatePet();
			var context = new TestLambdaContext();
			var requestUser = new PetModel
			{
				Name = "Tati 2",
				PetType = PetType.Dog,
				IdOwner = "LAZzUWoBfKkxXf3ErZ77",
				OwnerType = PersonType.PF,
				Description = "Achei na rua",
				PetAge = PetAge.Adult,
				Color = "Branca"
			};
			var apiGateway = new APIGatewayProxyRequest
			{
				HttpMethod = "POST",
				Path = "create-pet",
				Body = JsonConvert.SerializeObject(requestUser),
				Headers = new Dictionary<string, string>()
				{
					{ "Content-Type", "application/json" }
				}
			};
			var _return = function.FunctionHandler(apiGateway, context);
			Assert.Equal(200, _return.StatusCode);
		}

		[Fact]
		public void UpdatePet()
		{
			var function = new FunctionCreatePet();
			var context = new TestLambdaContext();
			var requestUser = new PetModel
			{
				Name = "Tati 3",
				PetType = PetType.Cat,
				IdOwner = "LAZzUWoBfKkxXf3ErZ77",
				OwnerType = PersonType.PF,
				Description = "Achei na rua",
				PetAge = PetAge.Adult,
				Color = "Listrado",
				Id = "MgYZiWoBfKkxXf3EE55r"
			};
			var apiGateway = new APIGatewayProxyRequest
			{
				HttpMethod = "PUT",
				Path = "create-pet",
				Body = JsonConvert.SerializeObject(requestUser),
				Headers = new Dictionary<string, string>()
				{
					{ "Content-Type", "application/json" }
				}
			};
			var _return = function.FunctionHandler(apiGateway, context);
			Assert.Equal(200, _return.StatusCode);
		}

		[Fact]
		public void GetPet()
		{
			var function = new FunctionGetPet();
			var context = new TestLambdaContext();
			var request = new PetRequest
			{
				id = "LgbAVmoBfKkxXf3E5p5A"
			};
			var apiGateway = new APIGatewayProxyRequest
			{
				HttpMethod = "GET",
				Path = "get-pet",
				Resource = "/dev/",
				Body = JsonConvert.SerializeObject(request),
				Headers = new Dictionary<string, string>()
				{
					{ "Content-Type", "application/json" }
				}
			};
			var _return = function.FunctionHandler(apiGateway, context);
			Assert.Equal(200, _return.StatusCode);
		}

		[Fact]
		public void ListPet()
		{
			var function = new FunctionListPet();
			var context = new TestLambdaContext();
			var apiGateway = new APIGatewayProxyRequest
			{
				HttpMethod = "GET",
				Path = "list-pet",
				Resource = "/dev/",
				Headers = new Dictionary<string, string>()
				{
					{ "Content-Type", "application/json" }
				},
				QueryStringParameters = new Dictionary<string, string> { { "idOwner", "LAZzUWoBfKkxXf3ErZ7" } }
			};
			var _return = function.FunctionHandler(apiGateway, context);
			Assert.Equal(200, _return.StatusCode);
		}

		[Fact]
		public void Search()
		{
			var function = new FunctionSearch();
			var context = new TestLambdaContext();
			var request = new QueryModel
			{
				PetType = (int)PetType.Cat,
				PetAge = (int)PetAge.Baby,
				Color = "preto",
				id = "cwZJOWsBfKkxXf3ETJ4k"
			};
			var apiGateway = new APIGatewayProxyRequest
			{
				HttpMethod = "POST",
				Path = "search",
				Body = JsonConvert.SerializeObject(request),
				Headers = new Dictionary<string, string>()
				{
					{ "Content-Type", "application/json" }
				}
			};
			var _return = function.FunctionHandler(apiGateway, context);
			Assert.Equal(200, _return.StatusCode);
		}

		[Fact]
		public void Upload()
		{
			var function = new FunctionUploadPhotos();
			var context = new TestLambdaContext();

			byte[] imageArray = System.IO.File.ReadAllBytes(@"C:\Users\edwin.cavina\Desktop\edwin.jpg");
			string base64 = Convert.ToBase64String(imageArray);

			var request = new UploadModel
			{
				Id = "OgbZCmsBfKkxXf3E1J6h",
				ImgBase64 = base64
			};
			var apiGateway = new APIGatewayProxyRequest
			{
				HttpMethod = "POST",
				Path = "upload-photos",
				Body = JsonConvert.SerializeObject(request),
				Headers = new Dictionary<string, string>()
				{
					{ "Content-Type", "application/json" }
				}
			};
			var _return = function.FunctionHandler(apiGateway, context);
			Assert.Equal(200, _return.StatusCode);
		}

		[Fact]
		public void UpdateTransaction()
		{
			var function = new FunctionCreateTransaction();
			var context = new TestLambdaContext();
			var request = new TransactionModel
			{
				Id = "RwZJJWsBfKkxXf3EFp4U",
				Transaction = StatusTransaction.Chatting
			};
			var apiGateway = new APIGatewayProxyRequest
			{
				HttpMethod = "PUT",
				Path = "create-transaction",
				Body = JsonConvert.SerializeObject(request),
				Headers = new Dictionary<string, string>()
				{
					{ "Content-Type", "application/json" }
				}
			};
			var _return = function.FunctionHandler(apiGateway, context);
			Assert.Equal(200, _return.StatusCode);
		}

		[Fact]
		public void GetMessage()
		{
			var function = new FunctionGetMessage();
			var context = new TestLambdaContext();
			var apiGateway = new APIGatewayProxyRequest
			{
				HttpMethod = "GET",
				Path = "get-message",
				Resource = "/dev/",
				Headers = new Dictionary<string, string>()
				{
					{ "Content-Type", "application/json" }
				},
				QueryStringParameters = new Dictionary<string, string> { { "id", "bwZ2NGsBfKkxXf3Ebp6g" } }
			};
			var _return = function.FunctionHandler(apiGateway, context);
			Assert.Equal(200, _return.StatusCode);
		}

		[Fact]
		public void ListTransaction()
		{
			var function = new FunctionListTransaction();
			var context = new TestLambdaContext();
			var apiGateway = new APIGatewayProxyRequest
			{
				HttpMethod = "GET",
				Path = "list-transaction",
				Resource = "/dev/",
				Headers = new Dictionary<string, string>()
				{
					{ "Content-Type", "application/json" }
				},
				QueryStringParameters = new Dictionary<string, string> { { "id", "fgbOPGsBfKkxXf3E4Z5y" } }
			};
			var _return = function.FunctionHandler(apiGateway, context);
			Assert.Equal(200, _return.StatusCode);
		}
	}
}
