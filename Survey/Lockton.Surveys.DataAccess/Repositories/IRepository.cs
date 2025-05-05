using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lockton.Surveys.DataAccess.Repositories
{
    public interface IRepository<T> where T : class
    {
        ValueTask<EntityEntry<T>> Create(T entity);
        void Delete(T entity);
        IQueryable<T> GetAll();
        IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression);
        Task<int> SaveChanges();
        void Update(T entity);
        IDbContextTransaction BeginTransaction();
        void CommitTransaction();
        DbContext GetContext();
        DataSet GetDataSet(string query, bool isProcedure = true, List<SqlParameter> parameters = null);
        IQueryable<T> ExecuteSP(string query, params object[] parameters);
        IQueryable<T> ExecuteSP(string query);
        void ExecuteNonQuery(string query);
        void ExecuteNonQuery(string query, params object[] parameters);
    }
}
