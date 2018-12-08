CREATE SCHEMA [HR]
GO
CREATE ROLE [HR]
GO
Use BIZ_DEV
GO
Create table HR.Employee(
	EmployeePK			numeric(18,0) identity(1,1),
	EmployeeID			varchar(30) not null,
	EmployedDate		datetime,
	FirstName			varchar(250) null,
	LastName			varchar(250) null,
	Email				varchar(250) null,
	PhoneNo				numeric(18,0) null,
	Addressline1		varchar(50) null,
	Addressline2		varchar(50) null,
	City				varchar(50) null,
	Statecode			varchar(50) null,
	country				varchar(15) null,
	pincode				varchar(15) null,
	Gender				tinyint,
	DOB					datetime,
	IDType				tinyint,
	ReferenceID			varchar(50),
	DeptType			smallint,
	Designation			smallint,
	Location			smallint,
	ManagerEmpFK		numeric(18,0),
	ContactName			varchar(250),
	ContactPhoneNo		numeric(18,0),
	ISAppUser			bit,
	AppuserFK			numeric(18,0),

	constraint PK_Employee primary key clustered (EmployeePK)
)

Insert into Master.CommonCode(CodeType, CodeID, Code, IsActive)
values('GENDER', 1, 'Male', 1)
GO
Insert into Master.CommonCode(CodeType, CodeID, Code, IsActive)
values('GENDER', 2, 'Female', 1)
GO
Insert into Master.CommonCode(CodeType, CodeID, Code, IsActive)
values('IDPROOFTYPE', 1, 'Passport', 1)
GO
Insert into Master.CommonCode(CodeType, CodeID, Code, IsActive)
values('IDPROOFTYPE', 2, 'Pan Card', 1)
GO
Insert into Master.CommonCode(CodeType, CodeID, Code, IsActive)
values('IDPROOFTYPE', 3, 'Voter ID', 1)
GO
Insert into Master.CommonCode(CodeType, CodeID, Code, IsActive)
values('IDPROOFTYPE', 4, 'Aadhaar Card', 1)
GO
Insert into Master.CommonCode(CodeType, CodeID, Code, IsActive)
values('IDPROOFTYPE', 5, 'Driving License', 1)
GO
Insert into Master.CommonCode(CodeType, CodeID, Code, IsActive)
values('OCCUPANCYTYPE', 1, 'Owned', 1)
GO
Insert into Master.CommonCode(CodeType, CodeID, Code, IsActive)
values('OCCUPANCYTYPE', 2, 'Leased', 1)
GO
Insert into Master.CommonCode(CodeType, CodeID, Code, IsActive)
values('OCCUPANCYTYPE', 3, 'Rented', 1)
GO
Insert into Master.CommonCode(CodeType, CodeID, Code, IsActive)
values('DEPARTMENT', 1, 'Purchase', 1)
GO
Insert into Master.CommonCode(CodeType, CodeID, Code, IsActive)
values('DEPARTMENT', 2, 'Sales', 1)
GO
Insert into Master.CommonCode(CodeType, CodeID, Code, IsActive)
values('DEPARTMENT', 3, 'Accounts', 1)
GO
Insert into Master.CommonCode(CodeType, CodeID, Code, IsActive)
values('DEPARTMENT', 4, 'Delivery', 1)
GO
Insert into Master.CommonCode(CodeType, CodeID, Code, IsActive)
values('DEPARTMENT', 5, 'Marketing', 1)
GO
Insert into Master.CommonCode(CodeType, CodeID, Code, IsActive)
values('DEPARTMENT', 6, 'Administration', 1)
GO
Insert into Master.CommonCode(CodeType, CodeID, Code, IsActive)
values('BUSNTYPE', 1, 'Sole proprietorship', 1)
GO
Insert into Master.CommonCode(CodeType, CodeID, Code, IsActive)
values('BUSNTYPE', 2, 'Partnership', 1)
GO
Insert into Master.CommonCode(CodeType, CodeID, Code, IsActive)
values('BUSNTYPE', 3, 'Private Limited', 1)
GO
Insert into Master.CommonCode(CodeType, CodeID, Code, IsActive)
values('BUSNTYPE', 4, 'Public Limited', 1)
GO
Insert into Master.CommonCode(CodeType, CodeID, Code, IsActive)
values('STATEMENTTYP', 1, 'Balance Sheet', 1)
GO
Insert into Master.CommonCode(CodeType, CodeID, Code, IsActive)
values('STATEMENTTYP', 2, 'Income Statement', 1)
GO

Create table AppAdmin.CompanyLocation(
	CompanyLocationPK	smallint identity(1,1),
	CompanyFK			smallint,
	Location_Name		varchar(50) not null,
	Usage				varchar(15) not null,
	Addressline1		varchar(50) null,
	Addressline2		varchar(50) null,
	City				varchar(50) null,
	Statecode			varchar(50) null,
	country				varchar(15) null,
	pincode				varchar(15) null,
	Email				varchar(250) null,
	PhoneNo				numeric(18,0) null,
	Reg_No				varchar(30) null,
	OccupancyType		tinyint,
	Started_Date		datetime null,
	IsActive			bit not null,	
	constraint PK_CompanyLocation primary key clustered (CompanyLocationPK)
)

Insert into AppAdmin.CompanyLocation(CompanyFK,Location_Name, Usage, Addressline1, Addressline2, City, Statecode, country, pincode, OccupancyType, Started_Date, ISActive)
values(1,'Saravana Stores', 'Retail', '212 Arni Road','','Chetpet', 'Tamil Nadu', 'India', '606801', 1, '2003-06-05',1)
GO
Insert into AppAdmin.CompanyLocation(CompanyFK,Location_Name, Usage, Addressline1, Addressline2, City, Statecode, country, pincode, OccupancyType, Started_Date, ISActive)
values(1,'Saravana Stores Warehouse', 'Inventory', '32 Nehru St','','Chetpet', 'Tamil Nadu', 'India', '606801', 1, '2003-06-05',1)
GO

Create table Master.Currency(
	CurrencyPK			tinyint identity(1,1),
	Currency			varchar(50) not null,
	Currency_Symbol		varchar(15),
	Decimal_Name		varchar(15),
	IsActive			bit not null,	
	constraint PK_Currency primary key clustered (CurrencyPK)
)

Insert into Master.Currency(Currency, Currency_Symbol, Decimal_Name, IsActive)
values('Indian Rupees', 'INR', 'Paise',1)
Go
Insert into Master.Currency(Currency, Currency_Symbol, Decimal_Name, IsActive)
values('US Dollar', '$', 'cents',1)
Go

USE [BIZ_DEV]
GO

/****** Object:  Schema [Master]    Script Date: 3/23/2018 1:23:31 PM ******/
CREATE SCHEMA [Sales]
GO
CREATE SCHEMA [Purchase]
GO
CREATE ROLE [Sales]
GO
CREATE ROLE [Purchase]
GO
create table Purchase.Vendor(
	VendorPK			numeric(18,0) identity(1,1),
	CompanyFK			smallint,
	VendorName			varchar(250) not null,
	BusnType			tinyint,
	Tax_No				varchar(30) null,
	Addressline1		varchar(50) null,
	Addressline2		varchar(50) null,
	City				varchar(50) null,
	Statecode			varchar(50) null,
	country				varchar(15) null,
	pincode				varchar(15) null,
	Email				varchar(250) null,
	PhoneNo				numeric(18,0) null,
	CurrencyPK			tinyint,
	CreatedLocationfk	smallint,
	ContactName1		varchar(250) not null,
	Contact1Extn		numeric(18,0) null,
	Contact1PhoneNo		numeric(18,0) null,
	ContactName2		varchar(250) not null,
	Contact2Extn		numeric(18,0) null,
	Contact2PhoneNo		numeric(18,0) null,
	Created_Date		datetime null,
	constraint PK_Vendor primary key clustered (VendorPK)
)

Create table Sales.Customer(
	CustomerPK			numeric(18,0) identity(1,1),
	CompanyFK			smallint,
	FirstName			varchar(25) not null,
	LastName			varchar(250) not null,
	BusnType			tinyint,
	Tax_No				varchar(30) null,
	Addressline1		varchar(50) null,
	Addressline2		varchar(50) null,
	City				varchar(50) null,
	Statecode			varchar(50) null,
	country				varchar(15) null,
	pincode				varchar(15) null,
	Email				varchar(250) null,
	PhoneNo				numeric(18,0) null,
	MobileNo			numeric(18,0) null,
	Gender				tinyint,
	DOB					datetime,
	IDType				tinyint,
	ReferenceID			varchar(50),
	CreatedLocationfk	smallint,
	Created_Date		datetime null,
	constraint PK_Customer primary key clustered (CustomerPK)
)
GO
CREATE SCHEMA [Accounts]
GO
GO
CREATE ROLE [Accounts]
GO
Create table Accounts.GroupMaster(
	GroupMasterPK		smallint identity(1,1),
	CompanyFK			smallint,
	GroupName			varchar(50) not null,
	TopGroupFK			smallint,
	StatementType		tinyint,
	Statementpart		tinyint,
	Groupprefix			smallint,
	Displaysequence		smallint,
	Createdby			varchar(30),
	CreatedDate			datetime null,
	Updatedby			varchar(30),
	UpdatedDate			datetime null,
	constraint PK_GroupMaster primary key clustered (GroupMasterPK)
)
