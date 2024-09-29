using System.ComponentModel.DataAnnotations;
using System;
using DataAccessLayer.Models;
using System.Collections.Generic;

namespace MVC.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "code is Required")]//client-side (when use jquery-Validation) validation
        public int Code { get; set; }
        //.
        [Required(ErrorMessage = "Name is Required")]//client-side (when use jquery-Validation) validation
        public string Name { get; set; }/*]*/
        public DateTime DateOfCreation { get; set; }//DateTime is value type so it is required by default(not allow null)
        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();//Navigation property
    }
}
