using System.ComponentModel.DataAnnotations;

namespace MVC.ViewModels
{
	public class RestPasswordViewModel
	{
		[Required(ErrorMessage = "Password Is Required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Required(ErrorMessage = "Confirm Password Is Required")]
		[Compare(nameof(Password), ErrorMessage = "Confirm Password doesn't match Password")]//to be same as password
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }
	}
}