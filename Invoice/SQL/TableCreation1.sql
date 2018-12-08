Use BIZDB
GO
Create table Tenant(
	TenantPK		numeric(18,0) identity(1,1),
	FirstName		varchar(250),
	LastName		varchar(250),
	Email			varchar(250),
	PhoneNo			numeric(18,0),
	CompanyName		varchar(250),
	TenantName		varchar(500),
	UserID			varchar(30),
	constraint PK_Tenant primary key clustered (TenantPK)
)
GO

Use BIZ_DEV
GO
Create table AppAdmin.AppUser(
	AppUserPK		numeric(18,0) identity(1,1),
	UserID			varchar(30) not null,
	UserName		varchar(500) not null,
	FirstName		varchar(250) null,
	LastName		varchar(250) null,
	Email			varchar(250) null,
	Passkey			varbinary(500),
	Logged			numeric(18,0),
	UserRole		tinyint	,
	LoginAttempt	tinyint,
	Lastlogin		datetime,	
	Lastlogout		datetime,	
	UserStatus		tinyint,
	constraint PK_AppUser primary key clustered (AppUserPK)
)
GO
Create table Master.UserRole(
	UserRolePK		tinyint not null,
	UserRole		varchar(50) not null,
	IsActive		bit not null,	
	constraint PK_UserRole primary key clustered (UserRolePK)
)
GO
Create table Master.CommonCode(
	CommonCodePK	numeric(18,0) identity(1,1),
	CodeType		varchar(15),
	CodeID			tinyint,	
	Code			varchar(50),
	IsActive		bit		,
	constraint PK_CommonCode primary key clustered (CommonCodePK)
)
GO
Insert into Master.UserRole(UserRolePK, UserRole, IsActive)
values(1, 'App Admin', 1)
GO

Insert into Master.UserRole(UserRolePK, UserRole, IsActive)
values(2, 'App User', 1)
GO

Insert into Master.CommonCode(CodeType, CodeID, Code, IsActive)
values('USERSTATUS', 1, 'Active', 1)
GO

Insert into Master.CommonCode(CodeType, CodeID, Code, IsActive)
values('USERSTATUS', 2, 'InActive', 1)
GO

Insert into Master.CommonCode(CodeType, CodeID, Code, IsActive)
values('USERSTATUS', 3, 'Locked', 1)
GO
