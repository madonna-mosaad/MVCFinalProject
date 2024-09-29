using DataAccessLayer.Models;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace MVC.ViewModels
{
    public class EmployeeViewModel
    {
        //same coments as Department
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50, ErrorMessage = "maximum length is 50")]
        [MinLength(4, ErrorMessage = "minimum length is 50")]
        public string Name { get; set; }
        [Range(21, 60, ErrorMessage = "the range is 21 to 60")]
        public int? Age { get; set; }
        [RegularExpression(@"^\d{1,3}-[a-zA-Z]{4,}-[a-zA-Z]{4,}-[a-zA-Z]{4,}$",
            ErrorMessage = "Address must be like 123-street-city-country")]
        public string Address { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Phone Number")]
        [Phone]
        public string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        //soft Delete ( lw eldata msh 3ayzaha f elwebsite bs 3ayzaha f elDB yb2a mms7hash la a3ml el IsDeleted b true)
        public Gender Gender { get; set; }
        //h7tag el FK f el view
        public Department? Department { get; set; }
        public int? DepartmentId { get; set; }
        public string? ImageName {  get; set; }
        public IFormFile? Image { get; set; }
    }
}
