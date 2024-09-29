using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Department:ModelBase
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public DateTime DateOfCreation { get; set; }//DateTime is value type so it is required by default(not allow null)
        public ICollection<Employee> Employees { get; set; }=new HashSet<Employee>();//Navigation property
    }
}

