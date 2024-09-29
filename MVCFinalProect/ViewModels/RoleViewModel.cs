using System;
using System.ComponentModel.DataAnnotations;

namespace MVC.ViewModels
{
    public class RoleViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public RoleViewModel() 
        {
            Id=Guid.NewGuid().ToString();
        }
    }
}
