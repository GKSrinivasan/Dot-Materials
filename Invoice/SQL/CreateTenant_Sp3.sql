CREATE PROC [dbo].[USP_PUT_NewDataBase]  --[DBO].[USP_PUT_NewDataBase] 1
AS
BEGIN
DECLARE @TenantPK NUMERIC(18,0) = (SELECT MAX(TenantPK) FROM Tenant)
DECLARE @newDatabase VARCHAR(100) = 'BIZ_DEV'+CAST(@TenantPK AS VARCHAR(50))

DECLARE @Qry VARCHAR(100)='CREATE DATABASE '+@newDatabase

DECLARE @backuplocation VARCHAR(100)='C:\Program Files\Microsoft SQL Server\MSSQL14.SQLSERVER\MSSQL\Backup\BIZ_DEV_'+CAST(GETDATE() AS VARCHAR(50))+'.bak'
DECLARE @mdf VARCHAR(100)='C:\Program Files\Microsoft SQL Server\MSSQL14.SQLSERVER\MSSQL\DATA\BIZ_DEV_'+CAST(@TenantPK AS VARCHAR(100))+'.mdf'
DECLARE @log VARCHAR(100)='C:\Program Files\Microsoft SQL Server\MSSQL14.SQLSERVER\MSSQL\Log\BIZ_DEV_'+CAST(@TenantPK AS VARCHAR(100))+'.ldf'
DECLARE @userRole VARCHAR(10) = (SELECT UserRolePK FROM BIZ_DEV.Master.UserRole WHERE UserRole = 'App Admin' AND IsActive = 1)
DECLARE @userStatus VARCHAR(10) = (SELECT CommonCodePK FROM BIZ_DEV.Master.CommonCode WHERE CodeType = 'USERSTATUS' AND Code = 'Active' AND IsActive = 1 )
DECLARE @pswd VARBINARY(150) = CAST('xyz' AS VARBINARY)
DECLARE @insertQry VARCHAR(MAX)

EXEC(@Qry)

BACKUP DATABASE BIZ_DEV TO DISK = @backuplocation WITH INIT, COMPRESSION;

RESTORE DATABASE @newDatabase
  FROM DISK = @backuplocation
  WITH REPLACE,
  MOVE 'BIZ_DEV' TO @mdf,
  MOVE 'BIZ_DEV_log' TO @log;


SET @insertQry =
'INSERT INTO '+@newDatabase+'.AppAdmin.AppUser
SELECT UserID,FirstName+'' ''+ISNULL(LastName,''''),FirstName,LastName,Email,NULL,0,'+@userRole+',0,NULL,NULL,'+@userStatus+' FROM [BIZDB].[dbo].[Tenant] 
WHERE TenantPK='+CAST(@TenantPK AS VARCHAR(100))

EXEC(@insertQry)

DECLARE @ToEmail VARCHAR(350) = (SELECT Email FROM Tenant WHERE TenantPK = @TenantPK)


EXEC msdb.dbo.sp_send_dbmail
    @profile_name = 'DatabaseMail',
    @recipients = @ToEmail,
    @body = 'Hi,

			Welcome to "New Invoice Application". Please click the below link to login your tenant.

			http://localhost:53565/',
    @subject = 'Welcome Email',
    @importance ='HIGH'

SELECT 1 [Status]
END