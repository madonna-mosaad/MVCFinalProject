using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVC.ViewModels
{
	public class UsersViewModel
	{
		public string Id { get; set; }
        [Required(ErrorMessage = "FName Is Required")]
        public string FName { get; set; }
        [Required(ErrorMessage = "LName Is Required")]
        public string LName { get; set; }
        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "InValid Email")]
        public string Email { get; set; }
        [Required(ErrorMessage ="PhoneNumber is required")]
        public string PhoneNumber {  get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare(nameof(Password), ErrorMessage = "Confirm Password doesn't match Password")]//to be same as password
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public ICollection<string> Roles { get; set; }=new HashSet<string>();
		public UsersViewModel()
		{
			Id = Guid.NewGuid().ToString();
		}

	}
}