using System.ComponentModel.DataAnnotations;

namespace MVC.ViewModels
{
	public class SignUpViewModel
	{
		[Required(ErrorMessage = "FName Is Required")]
		public string FName { get; set; }
		[Required(ErrorMessage = "LName Is Required")]
		public string LName { get; set; }
		[Required(ErrorMessage = "Email Is Required")]
		[EmailAddress(ErrorMessage = "InValid Email")]
		public string Email { get; set; }
		[Required(ErrorMessage = "PhoneNumber is required")]
		public string PhoneNumber { get; set; }
		[Required(ErrorMessage = "Password Is Required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Required(ErrorMessage = "Confirm Password Is Required")]
		[Compare(nameof(Password), ErrorMessage = "Confirm Password doesn't match Password")]//to be same as password
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }
		[Required(ErrorMessage = "Required To Agree")]
		public bool IsAgree { get; set; }
	}
}
