﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InvoiceDataLayer.EntityFramework
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Core.EntityClient;
    using System.Data.Entity.Infrastructure;
    using System.Data.SqlClient;

    public partial class BIZ_DEVEntities : DbContext
    {
        public BIZ_DEVEntities(string database)
            : base(nameOrConnectionString: ConnectionString(database))
        {
        }

        private static string ConnectionString(string database)
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
            sqlBuilder.DataSource = "LAPTOP-HGVP3VEO\\SQLSERVER";
            sqlBuilder.InitialCatalog = database;
            sqlBuilder.PersistSecurityInfo = true;
            sqlBuilder.UserID = "sa";
            sqlBuilder.Password = "SQL@2017";
            sqlBuilder.MultipleActiveResultSets = true;

            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
            entityBuilder.ProviderConnectionString = sqlBuilder.ToString();
            entityBuilder.Metadata = "res://*/";
            entityBuilder.Provider = "System.Data.SqlClient";

            return entityBuilder.ToString();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<CommonCode> CommonCodes { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
    }
}
