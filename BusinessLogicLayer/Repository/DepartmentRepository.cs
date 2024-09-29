using BusinessLayer.Interfaces;
using DataAccessLayer.Data.Contexts;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Repository
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AppDbContext appDbContext) : base(appDbContext) { }
        public IQueryable<Department> GetByName(string name)
        {
            return _appDbContext.Departments.Where(x => x.Name.ToLower() == name.ToLower()).AsNoTracking();
        }
    }
}
