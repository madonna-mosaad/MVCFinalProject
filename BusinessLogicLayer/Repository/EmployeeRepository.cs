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
    public class EmployeeRepository :GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext appDbContext) : base(appDbContext) { }

        public IQueryable<Employee> GetByAddress(string address)
        {
           return _appDbContext.Employees.Where(e => e.Address.ToLower() == address.ToLower()).AsNoTracking();
        }
        public IQueryable<Employee> GetByName(string name)
        {
            return _appDbContext.Employees.Where(e => e.Name.ToLower() == name.ToLower()).Include(e => e.Department).AsNoTracking();
        }
    }
}
