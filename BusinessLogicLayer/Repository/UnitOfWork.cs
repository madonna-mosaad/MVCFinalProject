using BusinessLayer.Interfaces;
using DataAccessLayer.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Repository
{
    public class UnitOfWork : IUnitOfWork 
    {
        private readonly AppDbContext _dbContext;
        public IDepartmentRepository DepartmentRepository { get; set ; }
        public IEmployeeRepository EmployeeRepository { get; set; }
        public UnitOfWork(AppDbContext dbContext) 
        {
            _dbContext = dbContext;
            DepartmentRepository = new DepartmentRepository(_dbContext);
            EmployeeRepository = new EmployeeRepository(_dbContext);
        }

        public int Save()
        {
            return _dbContext.SaveChanges();
        }

        //the class that open connection with DB (Make CLR create object from dbcontext) ,will close the connection
        //CLR will call this method when the request end (because i make the IUnitOfWork implement IDisposable)
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
