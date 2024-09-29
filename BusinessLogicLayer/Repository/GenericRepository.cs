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
    public class GenericRepository<T> : IGenericRepository<T> where T : ModelBase
    {
        private protected readonly AppDbContext _appDbContext;
        public GenericRepository(AppDbContext appDbContext) 
        {
            _appDbContext = appDbContext;
        }
        public IEnumerable<T> GetAll()
        {
            if (typeof(T) == typeof(Employee))
            {
                //the casting because the method return IEnumrable<T> and without casting _appDb.... is IEnumrable<Employee>
                return (IEnumerable<T>)_appDbContext.Employees.Include(e => e.Department).AsNoTracking().ToList();
            }
            return _appDbContext.Set<T>().AsNoTracking().ToList();
        }

        public T GetById(int id)
        {
            return _appDbContext.Set<T>().Find(id);
        }

        //in GenericRepository before use UnitOfWork
        /*public int Add(T entity)
        {
            _appDbContext.Add(entity);
            return _appDbContext.SaveChanges();
        }

        public int Delete(T entity)
        {
            _appDbContext.Remove(entity);
            return _appDbContext.SaveChanges();
        }
        public int Update(T entity)
        {
            _appDbContext.Update(entity);
            return _appDbContext.SaveChanges();
        }*/

        //after use UnitOfWork
        public void Add(T entity)
        {
            _appDbContext.Add(entity);
        }

        public void Delete(T entity)
        {
            _appDbContext.Remove(entity);
        }
        public void Update(T entity)
        {
            _appDbContext.Update(entity);
        }

        
    }
}
