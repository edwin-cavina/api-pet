using Newtonsoft.Json;
using PET.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LambdaPETListTransaction
{
	class ListTransacton
	{
		[JsonProperty(PropertyName = "transaction_owner")]
		public List<TransactionModel> ListOwner { get; set; } = null;

		[JsonProperty(PropertyName = "transaction_adopter")]
		public List<TransactionModel> ListAdopter { get; set; } = null;
	}
}
