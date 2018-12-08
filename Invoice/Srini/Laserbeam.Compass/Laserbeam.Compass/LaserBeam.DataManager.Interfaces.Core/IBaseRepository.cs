
// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
//
// Component Name :   IBaseRepository
// Description    :   Interface for BaseRepository
// Author         :   Boobalan Ranganathan		
// Creation Date  :   11-Sep-2015

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Laserbeam.DataManager.Interfaces.Core
{
    public interface IBaseRepository : IDisposable
    {
        #region Get Query
        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   11-Sep-2015
        /// <summary>
        /// Gets queryable to retrive all records
        /// </summary>
        /// <typeparam name="TEntity">An entity type of the current DbContext</typeparam>
        /// <returns>Returns queryable of type TEntity</returns>
        IQueryable<TEntity> GetQuery<TEntity>() where TEntity : class;


        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   11-Sep-2015
        /// <summary>
        /// Gets queryable to retrive records matching whereClause
        /// </summary>
        /// <typeparam name="TEntity">An entity type of the current DbContext</typeparam>
        /// <param name="whereClause">An expression to filter records</param>
        /// <returns>Returns queryable of type TEntity</returns>
        IQueryable<TEntity> GetQuery<TEntity>(Expression<Func<TEntity, bool>> whereClause) where TEntity : class;

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   25-Sep-2015
        /// <summary>
        /// Gets queryable to retrive records along with the child entity
        /// </summary>
        /// <typeparam name="TEntity">An entity type of the current DbContext</typeparam>
        /// <param name="includeEntity">Name of a child entity</param>
        /// <returns>Returns queryable of type TEntity</returns>
        IQueryable<TEntity> GetQuery<TEntity>(string includeEntity) where TEntity : class;

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   25-Sep-2015
        /// <summary>
        /// Gets queryable to retrive records along with the child entity
        /// </summary>
        /// <typeparam name="TEntity">An entity type of the current DbContext</typeparam>
        /// <param name="includeEntity">Name of a child entity</param>
        /// <param name="whereClause">An expression to filter records</param>
        /// <returns>Returns queryable of type TEntity</returns>
        IQueryable<TEntity> GetQuery<TEntity>(string includeEntity, Expression<Func<TEntity, bool>> whereClause) where TEntity : class;

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   25-Sep-2015
        /// <summary>
        /// Gets queryable to retrive records along with the child entities
        /// </summary>
        /// <typeparam name="TEntity">An entity type of the current DbContext</typeparam>
        /// <param name="includeEntities">Array of child entity names</param>
        /// <returns>Returns queryable of type TEntity</returns>
        IQueryable<TEntity> GetQuery<TEntity>(string[] includeEntities) where TEntity : class;

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   25-Sep-2015
        /// <summary>
        /// Gets queryable to retrive records along with the child entities
        /// </summary>
        /// <typeparam name="TEntity">An entity type of the current DbContext</typeparam>
        /// <param name="includeEntities">Array of child entity names</param>
        /// <param name="whereClause">An expression to filter records</param>
        /// <returns>Returns queryable of type TEntity</returns>
        IQueryable<TEntity> GetQuery<TEntity>(string[] includeEntities, Expression<Func<TEntity, bool>> whereClause) where TEntity : class;

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
        Task<TEntity> GetData<TEntity>(int entityId) where TEntity : class;

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   22-Sep-2015

        // Modified By    :   Boobalan Ranganathan		
        // Modified Date  :   04-Jul-2016
        // Ticket ID      :   PSP-11680
        // Comment        :   Removed SqlClient dependency

        /// <summary>
        /// Gets records for the sql command
        /// </summary>
        /// <typeparam name="TEntity">An entity type that matches the sql query output</typeparam>
        /// <param name="commandText">Sql query or stored procedure</param>
        /// <param name="parameters">Array of DbParameters</param>
        /// <returns>Returns enumerable of type TEntity</returns>
        Task<IEnumerable<TEntity>> GetData<TEntity>(string commandText, DbParameter[] parameters);

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   22-Sep-2015
        /// <summary>
        /// Gets record for the stored procedure
        /// </summary>
        /// <param name="queryText">Name of the stored procedure</param>
        /// <returns>Returns DataTable</returns>
        Task<DataTable> GetDataTableFromStoredProcedure(string spName);

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
        Task<DataTable> GetDataTableFromStoredProcedure(string spName, DbParameter[] parametes);

        #endregion

        #region CURD Operations

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   09-Sep-2015
        /// <summary>
        /// Adds instance of TEntity to DbContext
        /// </summary>
        /// <typeparam name="TEntity">An entity type of the current DbContext</typeparam>
        /// <param name="entity">An instance of type TEntity</param>
        void Add<TEntity>(TEntity entity) where TEntity : class;

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   09-Sep-2015
        /// <summary>
        /// Adds collection of instances of TEntity to DbContext
        /// </summary>
        /// <typeparam name="TEntity">An entity type of the current DbContext</typeparam>
        /// <param name="entities">Enumerable of type TEntity</param>
        void Add<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   09-Sep-2015
        /// <summary>
        /// Edits instance of TEntity to DbContext
        /// </summary>
        /// <typeparam name="TEntity">An entity type of the current DbContext</typeparam>
        /// <param name="entity">An instance of type TEntity</param>
        void Edit<TEntity>(TEntity entity) where TEntity : class;

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   09-Sep-2015
        /// <summary>
        /// Edits collection of TEntity to DbContext
        /// </summary>
        /// <typeparam name="TEntity">An entity type of the current DbContext</typeparam>
        /// <param name="entities">Enumerable of type TEntity</param>
        void Edit<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   09-Sep-2015
        /// <summary>
        /// Delete instance of TEntity from DbContext
        /// </summary>
        /// <typeparam name="TEntity">An entity type of the current DbContext</typeparam>
        /// <param name="entity">An instance of type TEntity</param>
        void Delete<TEntity>(TEntity entity) where TEntity : class;

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   09-Sep-2015
        /// <summary>
        /// Delete instance of TEntity for the provided entity Id from DbContext
        /// </summary>
        /// <typeparam name="TEntity">An entity type of the current DbContext</typeparam>
        /// <param name="ID">Primary Id of TEntity</param>
        Task Delete<TEntity>(int ID) where TEntity : class;

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   09-Sep-2015
        /// <summary>
        /// Delete collection of instances of TEntity from DbContext
        /// </summary>
        /// <typeparam name="TEntity">An entity type of the current DbContext</typeparam>
        /// <param name="entities">Enumerable of type TEntity</param>
        void Delete<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   09-Sep-2015
        /// <summary>
        /// Saves all changes made in DbContext
        /// </summary>
        /// <param name="isValidateOnSave">Validates tracked entities automatically</param>
        /// <returns>Returns the number of records affected by save operation</returns>
        int SaveChanges(bool isValidateOnSave = true);

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   09-Sep-2015
        /// <summary>
        /// Saves all changes made in DbContext async
        /// </summary>
        /// <returns>Returns the number of records affected by save operation</returns>
        Task<int> SaveChangesAsync(bool isValidateOnSave = true);

        #endregion

        #region Execute

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   23-Sep-2015
        /// <summary>
        /// Executes a stored procedure
        /// </summary>
        /// <param name="spName">Name of the stored procedure</param>
        /// <returns>Returns number of rows affected</returns>
        Task<int> ExecuteStoredProcedure(string spName);

        // Author         :   Boobalan Ranganathan		
        // Creation Date  :   23-Sep-2015

        // Modified By    :   Boobalan Ranganathan		
        // Modified Date  :   04-Jul-2016
        // Ticket ID      :   PSP-11680
        // Comment        :   Removed SqlClient dependency

        /// <summary>
        /// Executes a stored procedure
        /// </summary>
        /// <param name="spName">Name of the stored procedure</param>
        /// <param name="param">Array of DbParameter for the stored procedure</param>
        /// <returns>Returns number of rows affected</returns>
        Task<int> ExecuteStoredProcedure(string spName, DbParameter[] param);

        Task<string> SqlBulkInsert(DataTable sheetData, string destinationTableName, List<string> columnsMapping);

        #endregion
    }
}
