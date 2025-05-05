using Lockton.Surveys.DataAccess.DataContext;
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
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private AppDbContext _context { get; set; }

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsNoTracking();
        }

        public virtual IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression).AsNoTracking();
        }

        public async ValueTask<EntityEntry<T>> Create(T entity)
        {
            return await _context.Set<T>().AddAsync(entity);
        }


        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }


        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
            //_context.Database.EnlistTransaction(transaction);
        }

        public void CommitTransaction()
        {
            _context.Database.CommitTransaction();
            //_context.Database.EnlistTransaction(transaction);
        }

        public DbContext GetContext()
        {
            return this._context;
        }

        public DataSet GetDataSet(string query, bool isProcedure = true, List<SqlParameter> parameters = null)
        {

            using (DataSet datos = new DataSet())
            {
                using (SqlConnection connection = new SqlConnection(this._context.Database.GetDbConnection().ConnectionString))
                {
                    using (SqlCommand comando = new SqlCommand(query, connection))
                    {

                        connection.Open();
                        comando.CommandTimeout = 0;
                        if (isProcedure)
                            comando.CommandType = CommandType.StoredProcedure;

                        if (parameters != null)
                            comando.Parameters.AddRange(parameters.ToArray());

                        using (SqlDataAdapter dtAdapter = new SqlDataAdapter(comando))
                        {
                            dtAdapter.Fill(datos);
                        }
                        connection.Close();
                    }
                }
                return datos.Copy();
            }
        }
        public IQueryable<T> ExecuteSP(string query, params object[] parameters)
        {
            var type = _context.Set<T>().FromSqlRaw(query, parameters);
            return type;
        }

        public IQueryable<T> ExecuteSP(string query)
        {
            var type = _context.Set<T>().FromSqlRaw(query);
            return type;
        }
        public void ExecuteNonQuery(string query)
        {
            _context.Database.ExecuteSqlRaw(query);
        }

        public void ExecuteNonQuery(string query, params object[] parameters)
        {
            _context.Database.ExecuteSqlRaw(query, parameters);
        }

    }
}
