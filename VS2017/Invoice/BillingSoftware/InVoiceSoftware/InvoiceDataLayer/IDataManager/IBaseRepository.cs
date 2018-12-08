using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace InvoiceDataLayer.IDataManager
{
    public interface IBaseRepository: IDisposable
    {
        #region Get Query
        IQueryable<TEntity> GetQuery<TEntity>() where TEntity : class;
        IQueryable<TEntity> GetQuery<TEntity>(Expression<Func<TEntity, bool>> whereClause) where TEntity : class;
        IQueryable<TEntity> GetQuery<TEntity>(string includeEntity) where TEntity : class;
        IQueryable<TEntity> GetQuery<TEntity>(string includeEntity, Expression<Func<TEntity, bool>> whereClause) where TEntity : class;
        IQueryable<TEntity> GetQuery<TEntity>(string[] includeEntities) where TEntity : class;
        IQueryable<TEntity> GetQuery<TEntity>(string[] includeEntities, Expression<Func<TEntity, bool>> whereClause) where TEntity : class;
        #endregion

        #region Get Data
        Task<TEntity> GetData<TEntity>(int entityId) where TEntity : class;
        Task<IEnumerable<TEntity>> GetData<TEntity>(string commandText, DbParameter[] parameters);
        Task<DataTable> GetDataTableFromStoredProcedure(string spName);
        Task<DataTable> GetDataTableFromStoredProcedure(string spName, DbParameter[] parametes);
        #endregion

        #region CURD Operations
        void Add<TEntity>(TEntity entity) where TEntity : class;
        void Add<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        void Edit<TEntity>(TEntity entity) where TEntity : class;
        void Edit<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        void Delete<TEntity>(TEntity entity) where TEntity : class;
        Task Delete<TEntity>(int ID) where TEntity : class;
        void Delete<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        int SaveChanges(bool isValidateOnSave = true);
        Task<int> SaveChangesAsync(bool isValidateOnSave = true);
        #endregion

        #region Execute
        Task<int> ExecuteStoredProcedure(string spName);
        Task<int> ExecuteStoredProcedure(string spName, DbParameter[] param);
        Task<string> SqlBulkInsert(DataTable sheetData, string destinationTableName, List<string> columnsMapping);
        #endregion
    }
}
