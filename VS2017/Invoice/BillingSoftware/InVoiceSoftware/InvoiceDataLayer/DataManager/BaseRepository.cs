using InvoiceDataLayer.IDataManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace InvoiceDataLayer.DataManager
{
    
    public class BaseRepository<TContext> : IMasterBaseRepository where TContext : DbContext
    {
        #region Fields
        private TContext dataContext;
        #endregion

        #region Constructors

        public BaseRepository(TContext context)
        {
            dataContext = context;
        }

        #endregion

        #region Get Query
        public IQueryable<TEntity> GetQuery<TEntity>() where TEntity : class
        {
            return dataContext.Set<TEntity>();
        }

        public IQueryable<TEntity> GetQuery<TEntity>(Expression<Func<TEntity, bool>> whereClause) where TEntity : class
        {
            return dataContext.Set<TEntity>().Where(whereClause);
        }

        public IQueryable<TEntity> GetQuery<TEntity>(string includeEntity) where TEntity : class
        {
            return dataContext.Set<TEntity>().Include(includeEntity);
        }

        public IQueryable<TEntity> GetQuery<TEntity>(string includeEntity, Expression<Func<TEntity, bool>> whereClause) where TEntity : class
        {
            return dataContext.Set<TEntity>().Include(includeEntity).Where(whereClause);
        }

        public IQueryable<TEntity> GetQuery<TEntity>(string[] includeEntities) where TEntity : class
        {
            DbQuery<TEntity> query = dataContext.Set<TEntity>();
            foreach (string entity in includeEntities)
            {
                query = query.Include(entity);
            }
            return query;
        }

        public IQueryable<TEntity> GetQuery<TEntity>(string[] includeEntities, Expression<Func<TEntity, bool>> whereClause) where TEntity : class
        {
            DbQuery<TEntity> query = dataContext.Set<TEntity>();
            foreach (string entity in includeEntities)
            {
                query = query.Include(entity);
            }
            return query.Where(whereClause);
        }

        #endregion

        #region Get Data

        public async Task<TEntity> GetData<TEntity>(int entityId) where TEntity : class
        {
            return await dataContext.Set<TEntity>().FindAsync(entityId);
        }

        public async Task<IEnumerable<TEntity>> GetData<TEntity>(string commandText, DbParameter[] parameters)
        {
            return await dataContext.Database.SqlQuery<TEntity>(commandText, parameters).ToListAsync();
        }

        public async Task<DataTable> GetDataTableFromStoredProcedure(string spName)
        {
            var connection = dataContext.Database.Connection;
            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();
            using (DbCommand dbCommand = connection.CreateCommand())
            {
                try
                {
                    DataTable table = new DataTable();
                    dbCommand.CommandText = spName;
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    using (var reader = await dbCommand.ExecuteReaderAsync())
                    {
                        table.Load(reader);
                    }
                    return table;
                }
                finally
                {
                    if (connection.State != ConnectionState.Closed)
                        connection.Close();
                }
            }
        }

        public async Task<DataTable> GetDataTableFromStoredProcedure(string spName, DbParameter[] parametes)
        {
            var connection = dataContext.Database.Connection;
            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();
            using (DbCommand dbCommand = connection.CreateCommand())
            {
                try
                {
                    DataTable table = new DataTable();
                    dbCommand.CommandText = spName;
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.AddRange(parametes);
                    using (var reader = await dbCommand.ExecuteReaderAsync())
                    {
                        table.Load(reader);
                    }
                    return table;
                }
                finally
                {
                    if (connection.State != ConnectionState.Closed)
                        connection.Close();
                }
            }
        }

        #endregion

        #region CRUD Operations

        public void Add<TEntity>(TEntity entity) where TEntity : class
        {
            dataContext.Set<TEntity>().Add(entity);
        }

        public void Add<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            dataContext.Set<TEntity>().AddRange(entities);
        }

        public void Edit<TEntity>(TEntity entity) where TEntity : class
        {
            dataContext.Entry(entity).State = EntityState.Modified;
        }

        public void Edit<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            foreach (var entity in entities)
            {
                dataContext.Entry(entity).State = EntityState.Modified;
            }
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity != null)
                dataContext.Set<TEntity>().Remove(entity);
        }

        public async Task Delete<TEntity>(int ID) where TEntity : class
        {
            var entity = await GetData<TEntity>(ID);
            if (entity != null)
                Delete<TEntity>(entity);
        }

        public void Delete<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            if (entities != null)
                dataContext.Set<TEntity>().RemoveRange(entities);
        }

        public int SaveChanges(bool isValidateOnSave = true)
        {
            dataContext.Configuration.ValidateOnSaveEnabled = isValidateOnSave;
            return dataContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync(bool isValidateOnSave = true)
        {
            dataContext.Configuration.ValidateOnSaveEnabled = isValidateOnSave;
            return await dataContext.SaveChangesAsync();
        }

        #endregion

        #region Execute

        public async Task<int> ExecuteStoredProcedure(string spName)
        {
            var connection = dataContext.Database.Connection;
            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();
            using (DbCommand dbCommand = connection.CreateCommand())
            {
                try
                {
                    dbCommand.CommandText = spName;
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    return await dbCommand.ExecuteNonQueryAsync();
                }
                finally
                {
                    if (connection.State != ConnectionState.Closed)
                        connection.Close();
                }
            }
        }

        public async Task<string> SqlBulkInsert(DataTable sheetData, string destinationTableName, List<string> columnsMapping)
        {
            var message = "";
            var connection = (SqlConnection)dataContext.Database.Connection;
            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            using (SqlTransaction sqlTran = connection.BeginTransaction())
            {
                try
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.CheckConstraints, sqlTran))
                    {
                        sqlBulkCopy.DestinationTableName = destinationTableName;
                        foreach (var columns in columnsMapping)
                        {
                            sqlBulkCopy.ColumnMappings.Add(columns, columns);
                        }
                        try
                        {
                            sqlBulkCopy.WriteToServer(sheetData);
                            sqlTran.Commit();
                        }
                        catch (SqlException ex)
                        {
                            sqlTran.Rollback();
                            message = ex.InnerException.ToString();
                        }
                        catch (Exception ex)
                        {
                            sqlTran.Rollback();
                            message = ex.InnerException.ToString();
                        }
                        finally
                        {
                            sqlBulkCopy.Close();
                        }
                    }
                }
                finally
                {
                    if (connection.State != ConnectionState.Closed)
                        connection.Close();
                }
            }

            return message;
        }

        public async Task<int> ExecuteStoredProcedure(string spName, DbParameter[] param)
        {
            var connection = dataContext.Database.Connection;
            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();
            using (DbCommand dbCommand = connection.CreateCommand())
            {
                try
                {
                    dbCommand.CommandText = spName;
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.AddRange(param);
                    return await dbCommand.ExecuteNonQueryAsync();
                }
                finally
                {
                    if (connection.State != ConnectionState.Closed)
                        connection.Close();
                }
            }
        }

        #endregion

        #region IDisposable Implementation

        public void Dispose()
        {
            dataContext.Dispose();
        }

        #endregion

    }
}

