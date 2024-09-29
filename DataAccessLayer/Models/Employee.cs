using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public enum Gender
    {
        [EnumMember(Value ="Male")]
        Male=1,
        [EnumMember(Value ="Female")]
        Female=2
    }

    public class Employee : ModelBase
    {
        //same coments as Department
        public string Name { get; set; }
        public int? Age { get; set; }
        public string Address { get; set; }
        public decimal Salary { get; set; }
        public bool IsActive {  get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        //soft Delete ( lw eldata msh 3ayzaha f elwebsite bs 3ayzaha f elDB yb2a mms7hash la a3ml el IsDeleted b true)
        public bool? IsDeleted { get; set; }
        public Gender Gender { get; set; }
        public Department Department { get; set; }
        public int? DepartmentId { get; set; }
        public string ImageName {  get; set; }
    }
}
