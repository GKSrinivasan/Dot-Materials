CREATE TABLE [dbo].[TenantDatabase](
	[TenantDatabasePK] [numeric](18, 0) NOT NULL PRIMARY KEY,
	[TenantFK] [int] NULL,
	[DataBaseName] [varchar](100) NULL,
)


