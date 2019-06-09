using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;

namespace PET.Core
{
	public class ReturnModel
	{
		public ReturnModel(object _return, bool success, List<string> message)
		{
			this.Result = _return;
			this.Success = success;
			this.Message = message;
		}
		public ReturnModel(object _return, bool success)
		{
			this.Result = _return;
			this.Success = success;
		}
		public ReturnModel(bool success, List<string> message)
		{
			this.Success = success;
			this.Message = message;
		}
		public ReturnModel(bool success)
		{
			this.Success = success;
		}

		[JsonProperty(PropertyName = "success")]
		public bool Success { get; set; }

		[JsonProperty(PropertyName = "message", NullValueHandling = NullValueHandling.Ignore)]
		public List<string> Message { get; set; }

		[JsonProperty(PropertyName = "result", NullValueHandling = NullValueHandling.Ignore)]
		public object Result { get; set; }
	}

	public class UserModel
	{
		[Required]
		[JsonProperty(PropertyName = "email")]
		public string Email { get; set; }

		[Required]
		[JsonProperty(PropertyName = "password")]
		public string Password { get; set; }

		[Required]
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[Required]
		[JsonProperty(PropertyName = "dateBirth")]
		public DateTime DateBirth { get; set; }

		[Required]
		[JsonProperty(PropertyName = "client_type")]
		public ClientType Client { get; set; }

		[Required]
		[JsonProperty(PropertyName = "person_type")]
		public PersonType Person { get; set; }

		[Required]
		[JsonProperty(PropertyName = "ddd_phone")]
		public int DDDPhone { get; set; }

		[Required]
		[JsonProperty(PropertyName = "phone")]
		public int Phone { get; set; }

		[Required]
		[JsonProperty(PropertyName = "cep")]
		public string CEP { get; set; }

		[Required]
		[JsonProperty(PropertyName = "address")]
		public string Address { get; set; }

		[Required]
		[JsonProperty(PropertyName = "address_complement")]
		public string AddressComplement { get; set; }

		[Required]
		[JsonProperty(PropertyName = "city")]
		public string City { get; set; }

		[Required]
		[JsonProperty(PropertyName = "state")]
		public string State { get; set; }

		[JsonProperty(PropertyName = "cpf")]
		public string Cpf { get; set; }

		[JsonProperty(PropertyName = "cnpj")]
		public string Cnpj { get; set; }

		[JsonProperty(PropertyName = "rg")]
		public string Rg { get; set; }

		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }
	}

	public class UserRequest
	{
		[JsonProperty(PropertyName = "email", NullValueHandling = NullValueHandling.Ignore)]
		public string Email { get; set; }

		[JsonProperty(PropertyName = "password", NullValueHandling = NullValueHandling.Ignore)]
		public string Password { get; set; }

		[JsonProperty(PropertyName = "id", NullValueHandling = NullValueHandling.Ignore)]
		public string id { get; set; }
	}

	public class PetRequest
	{
		[JsonProperty(PropertyName = "id")]
		public string id { get; set; }
	}

	public class PetModel 
	{
		[Required]
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[Required]
		[JsonProperty(PropertyName = "pet_type")]
		public PetType PetType { get; set; }

		[Required]
		[JsonProperty(PropertyName = "color")]
		public string Color { get; set; }

		[Required]
		[JsonProperty(PropertyName = "owner_type")]
		public PersonType OwnerType { get; set; }

		[Required]
		[JsonProperty(PropertyName = "description")]
		public string Description { get; set; }

		[Required]
		[JsonProperty(PropertyName = "pet_age")]
		public PetAge PetAge { get; set; }

		[Required]
		[JsonProperty(PropertyName = "id_owner")]
		public string IdOwner { get; set; }

		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }
	}

	public class TransactionModel
	{
		[JsonProperty(PropertyName = "id", NullValueHandling = NullValueHandling.Ignore)]
		public string Id { get; set; }

		[JsonProperty(PropertyName = "id_pet")]
		public string IdPet { get; set; }

		[JsonProperty(PropertyName = "id_adopter")]
		public string IdAdopter { get; set; }

		[JsonProperty(PropertyName = "id_owner")]
		public string IdOwner { get; set; }

		[JsonProperty(PropertyName = "status_transaction")]
		public StatusTransaction Transaction { get; set; }
	}
}
