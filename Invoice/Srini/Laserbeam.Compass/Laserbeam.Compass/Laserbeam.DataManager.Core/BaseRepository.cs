
// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
//
// Component Name :   BaseRepository
// Description    :   Provides entity I/O operation features
// Author         :   Boobalan Ranganathan		
// Creation Date  :   09-Sep-2015

using Laserbeam.DataManager.Interfaces.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
namespace Laserbeam.DataManager.Core
{
    public class BaseRepository<TContext> : IMasterBaseRepository where TContext : DbContext
    {
        #region Fields

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   09-Sep-2015
        /// <summary>
        /// Instance of the database context
        /// </summary>
        private TContext dataContext;

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   29-Sep-2015
        /// <summary>
        /// Instance of the TenantDataCacheProvide
        /// </summary>
        //private TenantDataCacheProvider dataCache;

        #endregion

        #region Constructors

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   09-Sep-2015
        /// <summary>
        /// Constructor to create instance of BaseRepository
        /// </summary>
        public BaseRepository(TContext context)
        {
            dataContext = context;
            //dataCache = tenantDataCache;
        }

        #endregion

        #region Get Query
        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   09-Sep-2015
        /// <summary>
        /// Gets queryable to retrive all records
        /// </summary>
        /// <typeparam name="TEntity">An entity type of the current DbContext</typeparam>
        /// <returns>Returns queryable of type TEntity</returns>
        public IQueryable<TEntity> GetQuery<TEntity>() where TEntity : class
        {
            return dataContext.Set<TEntity>();
        }

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   09-Sep-2015
        /// <summary>
        /// Gets queryable to retrive records matching whereClause
        /// </summary>
        /// <typeparam name="TEntity">An entity type of the current DbContext</typeparam>
        /// <param name="whereClause">An expression to filter records</param>
        /// <returns>Returns queryable of type TEntity</returns>
        public IQueryable<TEntity> GetQuery<TEntity>(Expression<Func<TEntity, bool>> whereClause) where TEntity : class
        {
            return dataContext.Set<TEntity>().Where(whereClause);
        }

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   25-Sep-2015
        /// <summary>
        /// Gets queryable to retrive records along with the child entity
        /// </summary>
        /// <typeparam name="TEntity">An entity type of the current DbContext</typeparam>
        /// <param name="includeEntity">Name of a child entity</param>
        /// <returns>Returns queryable of type TEntity</returns>
        public IQueryable<TEntity> GetQuery<TEntity>(string includeEntity) where TEntity : class
        {
            return dataContext.Set<TEntity>().Include(includeEntity);
        }

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   25-Sep-2015
        /// <summary>
        /// Gets queryable to retrive records along with the child entity
        /// </summary>
        /// <typeparam name="TEntity">An entity type of the current DbContext</typeparam>
        /// <param name="includeEntity">Name of a child entity</param>
        /// <param name="whereClause">An expression to filter records</param>
        /// <returns>Returns queryable of type TEntity</returns>
        public IQueryable<TEntity> GetQuery<TEntity>(string includeEntity, Expression<Func<TEntity, bool>> whereClause) where TEntity : class
        {
            return dataContext.Set<TEntity>().Include(includeEntity).Where(whereClause);
        }

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   25-Sep-2015
        /// <summary>
        /// Gets queryable to retrive records along with the child entities
        /// </summary>
        /// <typeparam name="TEntity">An entity type of the current DbContext</typeparam>
        /// <param name="includeEntities">Array of child entity names</param>
        /// <returns>Returns queryable of type TEntity</returns>
        public IQueryable<TEntity> GetQuery<TEntity>(string[] includeEntities) where TEntity : class
        {
            DbQuery<TEntity> query = dataContext.Set<TEntity>();
            foreach (string entity in includeEntities)
            {
                query = query.Include(entity);
            }
            return query;
        }

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   25-Sep-2015
        /// <summary>
        /// Gets queryable to retrive records along with the child entities
        /// </summary>
        /// <typeparam name="TEntity">An entity type of the current DbContext</typeparam>
        /// <param name="includeEntities">Array of child entity names</param>
        /// <param name="whereClause">An expression to filter records</param>
        /// <returns>Returns queryable of type TEntity</returns>
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

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   09-Sep-2015
        /// <summary>
        /// Gets a single record for the provided entityId
        /// </summary>
        /// <typeparam name="TEntity">An entity type of the current DbContext</typeparam>
        /// <param name="entityId">Primary Id of the entity</param>
        /// <returns>Returns instance of type TEntity</returns>
        public async Task<TEntity> GetData<TEntity>(int entityId) where TEntity : class
        {
            return await dataContext.Set<TEntity>().FindAsync(entityId);
        }

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   22-Sep-2015
        /// <summary>
        /// Gets records for the sql command
        /// </summary>
        /// <typeparam name="TEntity">An entity type that matches the sql query output</typeparam>
        /// <param name="commandText">Sql query or stored procedure</param>
        /// <param name="parameters">Array of DbParameters</param>
        /// <returns>Returns enumerable of type TEntity</returns>
        public async Task<IEnumerable<TEntity>> GetData<TEntity>(string commandText, DbParameter[] parameters)
        {
            return await dataContext.Database.SqlQuery<TEntity>(commandText, parameters).ToListAsync();
        }

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   22-Sep-2015

        // Modified By    :   Boobalan Ranganathan		
        // Modified Date  :   04-Jul-2016
        // Ticket ID      :   PSP-11680
        // Comment        :   Removed SqlClient dependency

        /// <summary>
        /// Gets record for the stored procedure
        /// </summary>
        /// <param name="queryText">Name of the stored procedure</param>
        /// <returns>Returns DataTable</returns>
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

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   22-Sep-2015

        // Modified By    :   Boobalan Ranganathan		
        // Modified Date  :   04-Jul-2016
        // Ticket ID      :   PSP-11680
        // Comment        :   Removed SqlClient dependency

        /// <summary>
        /// Gets record for the stored procedure
        /// </summary>
        /// <param name="queryText">Name of the stored procedure</param>
        /// <param name="parametes">Array of DbParameters</param>
        /// <returns>Returns DataTable</returns>
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

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   09-Sep-2015
        /// <summary>
        /// Adds instance of TEntity to DbContext
        /// </summary>
        /// <typeparam name="TEntity">An entity type of the current DbContext</typeparam>
        /// <param name="entity">An instance of type TEntity</param>
        public void Add<TEntity>(TEntity entity) where TEntity : class
        {
            dataContext.Set<TEntity>().Add(entity);
        }

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   09-Sep-2015
        /// <summary>
        /// Adds collection of instances of TEntity to DbContext
        /// </summary>
        /// <typeparam name="TEntity">An entity type of the current DbContext</typeparam>
        /// <param name="entities">Enumerable of type TEntity</param>
        public void Add<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            dataContext.Set<TEntity>().AddRange(entities);
        }

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   09-Sep-2015
        /// <summary>
        /// Edits instance of TEntity to DbContext
        /// </summary>
        /// <typeparam name="TEntity">An entity type of the current DbContext</typeparam>
        /// <param name="entity">An instance of type TEntity</param>
        public void Edit<TEntity>(TEntity entity) where TEntity : class
        {
            dataContext.Entry(entity).State = EntityState.Modified;
        }

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   09-Sep-2015
        /// <summary>
        /// Edits collection of TEntity to DbContext
        /// </summary>
        /// <typeparam name="TEntity">An entity type of the current DbContext</typeparam>
        /// <param name="entities">Enumerable of type TEntity</param>
        public void Edit<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            foreach (var entity in entities)
            {
                dataContext.Entry(entity).State = EntityState.Modified;
            }
        }

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   09-Sep-2015
        /// <summary>
        /// Delete instance of TEntity from DbContext
        /// </summary>
        /// <typeparam name="TEntity">An entity type of the current DbContext</typeparam>
        /// <param name="entity">An instance of type TEntity</param>
        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity != null)
                dataContext.Set<TEntity>().Remove(entity);
        }

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   09-Sep-2015
        /// <summary>
        /// Delete instance of TEntity for the provided entity Id from DbContext
        /// </summary>
        /// <typeparam name="TEntity">An entity type of the current DbContext</typeparam>
        /// <param name="ID">Primary Id of TEntity</param>
        public async Task Delete<TEntity>(int ID) where TEntity : class
        {
            var entity = await GetData<TEntity>(ID);
            if (entity != null)
                Delete<TEntity>(entity);
        }

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   09-Sep-2015
        /// <summary>
        /// Delete collection of instances of TEntity from DbContext
        /// </summary>
        /// <typeparam name="TEntity">An entity type of the current DbContext</typeparam>
        /// <param name="entities">Enumerable of type TEntity</param>
        public void Delete<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            if (entities != null)
                dataContext.Set<TEntity>().RemoveRange(entities);
        }

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   09-Sep-2015
        /// <summary>
        /// Saves all changes made in DbContext
        /// </summary>
        /// <param name="isValidateOnSave">Validates tracked entities automatically</param>
        /// <returns>Returns the number of records affected by save operation</returns>
        public int SaveChanges(bool isValidateOnSave = true)
        {
            dataContext.Configuration.ValidateOnSaveEnabled = isValidateOnSave;
            return dataContext.SaveChanges();
        }

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   09-Sep-2015
        /// <summary>
        /// Saves all changes made in DbContext async
        /// </summary>
        /// <returns>Returns the number of records affected by save operation</returns>
        public async Task<int> SaveChangesAsync(bool isValidateOnSave = true)
        {
            dataContext.Configuration.ValidateOnSaveEnabled = isValidateOnSave;
            return await dataContext.SaveChangesAsync();
        }

        #endregion

        #region Execute

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   23-Sep-2015

        // Modified By    :   Boobalan Ranganathan		
        // Modified Date  :   04-Jul-2016
        // Ticket ID      :   PSP-11680
        // Comment        :   Removed catch block

        /// <summary>
        /// Executes a stored procedure
        /// </summary>
        /// <param name="spName">Name of the stored procedure</param>
        /// <returns>Returns number of rows affected</returns>
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
        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   23-Sep-2015

        // Modified By    :   Boobalan Ranganathan		
        // Modified Date  :   04-Jul-2016
        // Ticket ID      :   PSP-11680
        // Comment        :   Removed catch block

        /// <summary>
        /// Executes a stored procedure
        /// </summary>
        /// <param name="spName">Name of the stored procedure</param>
        /// <param name="param">Array of DbParameters for the stored procedure</param>
        /// <returns>Returns number of rows affected</returns>
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

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   30-Sep-2015
        /// <summary>
        /// Disposes all disposable objects
        /// </summary>
        public void Dispose()
        {
            dataContext.Dispose();
        }

        #endregion

    }
}

