using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    //class to Make us Know which classes is Model (Table in DB) to control the T in IGenericRepository
    public class ModelBase
    {
        public int Id { get; set; }
    }
}
