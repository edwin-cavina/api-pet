using System.ComponentModel;

namespace PET.Core
{
	public enum ClientType
	{
		[Description("Adotador")]
		Adopter = 1,

		[Description("Cuidador")]
		Caretaker = 2
	}

	public enum PersonType
	{
		[Description("Pessoa Fisica")]
		PF = 1,

		[Description("Pessoa Juridica")]
		PJ = 2
	}

	public enum PetType
	{
		[Description("Gato")]
		Cat = 1,

		[Description("Cachorro")]
		Dog = 2
	}

	public enum PetAge
	{
		[Description("Filhote")]
		Baby = 1,

		[Description("Adulto")]
		Adult = 2
	}

	public enum StatusTransaction 
	{
		[Description("Aprovação Pendente")]
		PendingApproval = 1,
		[Description("Conversando")]
		Chatting = 2,
		[Description("Adotado")]
		Adopted = 3,
		[Description("Negado")]
		Denied = 4
	}
}
