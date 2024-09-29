using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IGenericRepository <T> where T : ModelBase //ModelBase class or any class inherit the ModelBase to make sure that the class is Model(Table in DB)
    {
        public IEnumerable<T> GetAll();
        public T GetById(int id);

        //in GenericRepository before use UnitOfWork
        //public int Add(T entity); // int because return of savechanges is the number of raw effected (lw 0 yb2a m3mlsh el add aw kda )
        //public int Update(T entity);
        //public int Delete(T entity);

        //after use UnitOfWork
        public void Add(T entity); 
        public void Update(T entity);
        public void Delete(T entity);
    }
}
