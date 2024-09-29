using System.ComponentModel.DataAnnotations;

namespace MVC.ViewModels
{
	public class SignInViewModel
	{
		[Required(ErrorMessage = "Email Is Required")]
		[EmailAddress(ErrorMessage = "InValid Email")]
		public string Email { get; set; }
		[Required(ErrorMessage = "Password Is Required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Required(ErrorMessage = "Required To Agree")]
		public bool IsAgree { get; set; }
	}
}
