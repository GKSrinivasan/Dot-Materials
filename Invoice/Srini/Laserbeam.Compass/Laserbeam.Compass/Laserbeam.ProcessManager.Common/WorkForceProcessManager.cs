// Copyright (c) 2017 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
// Component Name  : 	WorkForce Process Manager
// Description     : 	WorkForce related business logics
// Author          :    Raja Ganapathy
// Creation Date   :    30-Mar-2017 

using Laserbeam.BusinessObject.Common;
using Laserbeam.BusinessObject.Common.MeritBusinussObjects;
using Laserbeam.DataManager.Interfaces.Common;
using Laserbeam.EntityManager.Common;
using Laserbeam.ProcessManager.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Threading.Tasks;

namespace Laserbeam.ProcessManager.Common
{
    public class WorkForceProcessManager : IWorkForceProcessManager
    {
        #region Fields
        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// Object of IWorkForceRepository
        /// </summary>
        private IWorkForceRepository m_workForceRepository;
        #endregion

        #region Constructors
        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// Initialize the repository
        /// </summary>
        /// <param name="workForceProcessManager">Object of IWorkForceRepository</param>        
        public WorkForceProcessManager(IWorkForceRepository workForceRepository)
        {
            m_workForceRepository = workForceRepository;
        }

        #endregion
        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// To get the template column details
        /// </summary>
        /// <returns>Returns the template fields</returns>
        public IQueryable<TemplateMetaColumns> GetTemplateMetaColumnDetails()
        {
            return m_workForceRepository.GetTemplateMetaColumnDetails().Select(x => new TemplateMetaColumns
            {
                TemplateMetaColumnID = x.MetaColumnID,
                DataFormat = x.DataFormat,
                SampleData = x.SampleData,
                DataLength = x.DataLength,
                FieldName = x.FieldName,
                AliasName = x.TemplateAliasName,
                FunctionalGroup = x.FunctionalGroup,
                FieldDescription = x.FieldDescription,
                IsEnabled = x.TemplateDisplay,
                IsTemplateColumn = x.IsTemplate,
                IsTemplateDefaultColumn=x.IsTemplateDefaultValue ?? false,
                IsMandate = x.IsMandate,
                ControlType = x.ControlType,
                PlaceHolder = x.PlaceHolder,
                FieldInformation = x.FieldInformation,
                ControlFormat = x.ControlFormat
            });
        }

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// To update the selected field list
        /// </summary>
        /// <param name="selectedFields">Defines the selected fields</param>
        public async Task<bool> UpdateSelectedTemplateColumns(List<int> selectedFields)
        {
           return  await  m_workForceRepository.UpdateSelectedTemplateColumns(selectedFields);
        }

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// To get the employee data template
        /// </summary>
        /// <returns>Returns the data table</returns>
        public async Task<DataTable> GetEmployeeDataTemplate()
        {
            return await m_workForceRepository.GetEmployeeDataTemplate();
        }

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// To get the employee list for employee search
        /// </summary>
        /// <param name="loggedInUserNum">Logged in user num</param>
        /// <returns>Returns the EmployeeList</returns>
        public IQueryable<EmployeeListModel> GetEmployeeList(int loggedInUserNum)
        {
            return m_workForceRepository.GetEmployeeList(loggedInUserNum);
        }

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// To get the workforce tile data
        /// </summary>
        /// <param name="loggedInUserNum">Defines logged in user num</param>
        /// <returns>Returns the workforce data</returns>
        public async Task<WorkForceTileData> GetWorkForceTileData(int loggedInUserNum)
        {
            return await m_workForceRepository.GetWorkForceTileData(loggedInUserNum);
        }

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// To get the uploaded employee data
        /// </summary>
        /// <param name="loggedInUserNum">loggedInUserNum</param>
        /// <param name="isMeritEligible">Defines all employees or Merit eligible only</param>
        /// <returns>Returns the DataTable</returns>
        public async Task<DataTable> GetUploadedEmployeeDetails(int loggedInUserNum, string isMeritEligible)
        {
            return await m_workForceRepository.GetUploadedEmployeeDetails(loggedInUserNum, isMeritEligible);
        }

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// To get the dropdown or Combobox details
        /// </summary>
        /// <param name="columnName">Field Name</param>
        /// <returns>Returns the list</returns>
        public async Task<IEnumerable<DropDownListItems>> GetSelectedDropDownDetails(string columnName)
        {
            return await m_workForceRepository.GetSelectedDropDownDetails(columnName);
        }

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// To get the searched employee details
        /// </summary>
        /// <param name="employeeID">Defines the employee ID</param>
        /// <returns>Returns the employee details</returns>
        public async Task<IEnumerable<EmployeeErrorData>> GetSearchedEmployeeDetails(string employeeID)
        {
            return await m_workForceRepository.GetSearchedEmployeeDetails(employeeID);
        }

        public int UpdateXmlProcess(string sheetName, string fileSourcePath, int recordCount, int userNum)
        {
            return m_workForceRepository.UpdateXmlProcess(sheetName, fileSourcePath, recordCount,  userNum);
        }

        public async Task<bool> InsertStagingTableData(DataTable userData, string DbName, List<string> columnsName, int xmlProcessNum)
        {
            return await m_workForceRepository.InsertStagingTableData(userData, DbName, columnsName, xmlProcessNum);
        }

        public async Task<int> ValidateEmployeeData(int userNum)
        {
            return await m_workForceRepository.ValidateEmployeeData(userNum);
        }

        public async Task<int> ProcessEmployeeData(int userNum)
        {
          return await  m_workForceRepository.ProcessEmployeeData(userNum);
        }

        public async Task<bool> ClearAllData(string tenantName)
        {
            return await m_workForceRepository.ClearAllData(tenantName);
        }

        public async Task<DataTable> GetEmployeeDataErrorExport()
        {
            return await m_workForceRepository.GetEmployeeDataErrorExport();
        }

        public async Task<List<TemplateErrorListModel>> GetEmployeeDataErrorList()
        {
            var result = await m_workForceRepository.GetEmployeeDataErrorList();
            return (from x in result
                    select new TemplateErrorListModel
                    {
                        Error = x.Error,
                        AffectedData = x.AffectedData,
                        HowToFix = x.HowToFix
                    }).ToList();

        }

        // Author       : Shaheena Shaik
        // Creation Date: 25-April-2017
        /// <summary>
        /// Getting Upload WorkFlow Grid data from database
        /// </summary>
        /// <param name="userNum">ID of LoggedInUserNum</param>
        /// <returns>Returning a view with grid details</returns>
        public async Task<IEnumerable<TemplateDataModel>> GetEmployeeDataGridDetails(int userNum)
        {
            return await m_workForceRepository.GetEmployeeDataGridDetails(userNum);
        }



        public async Task<int> DeleteEmployeeTemplateData(int xmlProcessNum)
        {
            return await m_workForceRepository.DeleteEmployeeTemplateData(xmlProcessNum);
        }


        public bool GetEmployeeCount(string employeeID)
        {
            return m_workForceRepository.GetEmployeeCount(employeeID);
        }

        public async Task<bool> UpdateEmployeeData(DataCorrectionModel data, int userNum, string currentYear)
        {
        EmployeeDetailsCorrection empDetails = new EmployeeDetailsCorrection();
        empDetails.EmployeeID=data.EmployeeID;
        empDetails.FirstName = (data.FirstName != "") ? data.FirstName : null;
        empDetails.MiddleName = (data.MiddleName != "") ? data.MiddleName : null;
        empDetails.LastName = (data.LastName != "") ? data.LastName : null;
        empDetails.PreferredName = (data.PreferredName != "") ? data.PreferredName : null;
        empDetails.Gender = (data.Gender != "") ? data.Gender : null;
        empDetails.JobCode = (data.JobCode != "") ? data.JobCode : null;
        empDetails.JobTitle = (data.JobTitle != "") ? data.JobTitle : null;
        empDetails.CurrentGrade = (data.CurrentGrade != "") ? data.CurrentGrade : null;
        empDetails.BusinessUnit = (data.BusinessUnit != "") ? data.BusinessUnit : null;
        empDetails.Function = (data.Function != "") ? data.Function : null;
        empDetails.Department = (data.Department != "") ? data.Department : null;
        empDetails.EmployeeClass = (data.EmployeeClass != "") ? data.EmployeeClass : null;
        empDetails.PayrollStatus = (data.PayrollStatus != "") ? data.PayrollStatus : null;
        empDetails.FLSAStatus = (data.FLSAStatus != "") ? data.FLSAStatus : null;
        empDetails.EmployeeStatus = (data.EmployeeStatus != "") ? data.EmployeeStatus : null;
        empDetails.FTE = data.FTE;
        empDetails.ActualWorkHours = data.ActualWorkHours;
        empDetails.TotalWorkHours = data.WorkHours;
        empDetails.WorkCountry = (data.WorkCountry != "") ? data.WorkCountry : null;
        empDetails.WorkLocation = (data.WorkLocation != "") ? data.WorkLocation : null;
        empDetails.HireDate = data.HireDate;
        empDetails.TerminationDate = data.TerminationDate;
        empDetails.SupervisorID = (data.SupervisorID != "") ? data.SupervisorID : null;
        empDetails.EmailAddress = (data.EmailAddress != "") ? data.EmailAddress : null;
        empDetails.PayCurrency = (data.PayCurrency != "") ? data.PayCurrency : null;
        empDetails.LastPayChangeDate = data.LastPayChangeDate;
        empDetails.LastPayChangeReason = (data.LastPayChangeReason != "") ? data.LastPayChangeReason : null;
        empDetails.CurrentHourlyRate = data.CurrentHourlyRate;
        empDetails.CurrentAnnualizedSalary = data.CurrentAnnalizedSalary;
        empDetails.CurrentAnnualSalary = data.CurrentAnnualSalary;
        empDetails.PayrollSource = (data.PayrollSource != "") ? data.PayrollSource : null;
        empDetails.SalaryMin = data.SalaryMin;
        empDetails.SalaryMid = data.SalaryMid;
        empDetails.SalaryMax = data.SalaryMax;
        empDetails.MeritProrationDate = data.MeritProrationDate;
        empDetails.MeritProrationFactor = data.MeritProrationFactor;
        empDetails.MeritPerformanceRating = data.MeritPerformanceRating;
        empDetails.MeritEligible = (data.MeritEligible != "") ? data.MeritEligible : null;
        empDetails.MeritPct = data.MeritPct;
        empDetails.MeritAmount = data.MeritAmount;
        empDetails.MeritIncreaseGuideline = (data.MeritIncreaseGuideline != "") ? data.MeritIncreaseGuideline : null;
        empDetails.MeritEffectiveDate = data.MeritEffectiveDate;
        empDetails.LumpSumPct = data.LumpSumPct;
        empDetails.LumpSumAmount = data.LumpSumAmount;
        empDetails.LumpsumEffectiveDate = data.LumpsumEffectiveDate;
        empDetails.PromotionEligible = (data.PromotionEligible != "") ? data.PromotionEligible : null;
        empDetails.PromotionPct = data.PromotionPct;
        empDetails.PromotionAmount = data.PromotionAmount;
        empDetails.PromoteTo = (data.PromoteTo != null) ? data.PromoteTo : null;
        empDetails.LastPromotionDate = data.LastPromotionDate;
        empDetails.PromotionEffectiveDate = data.PromotionEffectiveDate;
        empDetails.AdjustmentEligible = (data.AdjustmentEligible != "") ? data.AdjustmentEligible : null;
        empDetails.AdjustmentPct = data.AdjustmenPct;
        empDetails.AdjustmentAmount = data.AdjustmentAmount;
        empDetails.AdjustmentEffectiveDate = data.AdjustmentEffectiveDate;
        empDetails.NewSalary = data.NewSalary;
        empDetails.NewHourlyRate = data.NewHourlyRate;
            empDetails.Comparatio = data.Comparatio;
            empDetails.NewComparatio = data.NewComparatio;
             //empDetails.Comparatio = data.EmployeeStatus=="Annual" ? Decimal.Round(((data.CurrentAnnualSalary/data.SalaryMid)*100)??0,5): Decimal.Round(((data.CurrentHourlyRate / data.SalaryMid) * 100) ?? 0, 5);
            //empDetails.NewComparatio = data.EmployeeStatus == "Annual" ? Decimal.Round(((data.NewSalary / data.SalaryMid) * 100) ?? 0, 5) : Decimal.Round(((data.NewHourlyRate / data.SalaryMid) * 100) ?? 0, 5);
            //empDetails.NewComparatio = data.NewComparatio;
            empDetails.MoreInfo1 = (data.MoreInfo1 != "") ? data.MoreInfo1 : null;
        empDetails.MoreInfo2 = (data.MoreInfo2 != "") ? data.MoreInfo2 : null;
        empDetails.MoreInfo3 = (data.MoreInfo3 != "") ? data.MoreInfo3 : null;
        empDetails.MoreInfo4 = (data.MoreInfo4 != "") ? data.MoreInfo4 : null;
        empDetails.MoreInfo5 = (data.MoreInfo5 != "") ? data.MoreInfo5 : null;
        empDetails.MoreInfo6 = (data.MoreInfo6 != "") ? data.MoreInfo6 : null;
        empDetails.MoreInfo7 = (data.MoreInfo7 != "") ? data.MoreInfo7 : null;
        empDetails.MoreInfo8 = (data.MoreInfo8 != "") ? data.MoreInfo8 : null;
        empDetails.MoreInfo9 = (data.MoreInfo9 != "") ? data.MoreInfo9 : null;
        empDetails.MoreInfo10 = (data.MoreInfo10 != "") ? data.MoreInfo10 : null;
        empDetails.MoreInfo11 = (data.MoreInfo11 != "") ? data.MoreInfo11 : null;
        empDetails.MoreInfo12 = (data.MoreInfo12 != "") ? data.MoreInfo12 : null;
        empDetails.MoreInfo13 = (data.MoreInfo13 != "") ? data.MoreInfo13 : null;
        empDetails.MoreInfo14 = (data.MoreInfo14 != "") ? data.MoreInfo14 : null;
        empDetails.MoreInfo15 = (data.MoreInfo15 != "") ? data.MoreInfo15 : null;
        empDetails.MoreInfo16 = (data.MoreInfo16 != "") ? data.MoreInfo16 : null;
        empDetails.MoreInfo17 = (data.MoreInfo17 != "") ? data.MoreInfo17 : null;
        empDetails.MoreInfo18 = (data.MoreInfo18 != "") ? data.MoreInfo18 : null;
        empDetails.MoreInfo19 = (data.MoreInfo19 != "") ? data.MoreInfo19 : null;
        empDetails.MoreInfo20 = (data.MoreInfo20 != "") ? data.MoreInfo20 : null;
        empDetails.MoreInfo21 = (data.MoreInfo21 != "") ? data.MoreInfo21 : null;
        empDetails.MoreInfo22 = (data.MoreInfo22 != "") ? data.MoreInfo22 : null;
        empDetails.MoreInfo23 = (data.MoreInfo23 != "") ? data.MoreInfo23 : null;
        empDetails.UserNum = userNum;
        return await m_workForceRepository.UpdateEmployeeData(data.BudgetProrationFactor,empDetails,data.StagingNumber??0,currentYear);
        }

        public List<OrphanManagerDetails> GetOrphanEmployeeDetails()
        {
            return m_workForceRepository.GetOrphanEmployeeDetails();
        }

        public async Task<int> GetEmployeeDataErrorCount()
        {
            return await m_workForceRepository.GetEmployeeDataErrorCount();
        }
        public async Task<int> AssignEmployeeToCorporate(List<int> EmployeeJobNum)
        {
            return await m_workForceRepository.AssignEmployeeToCorporate(EmployeeJobNum);
        }

        public RuleConfiguration GetMeritConfiguration()
        {
            return m_workForceRepository.GetMeritConfiguration();
        }
        public IQueryable<RatingModel> GetRatings()
        {
            return m_workForceRepository.GetRatings();
        }
        public async Task<string> GetCirCularReference(string employeeID, string supervisorID, string payrollStatus)
        {
            return await m_workForceRepository.GetCirCularReference(employeeID, supervisorID, payrollStatus);
        }

        public async Task<IEnumerable<EmployeeErrorData>> GetEmployeeErrorRecordDetails(string errorType)
        {
            return await m_workForceRepository.GetEmployeeErrorRecordDetails(errorType);
        }

        public async Task<DataTable> GetExportXmlFile(int xmlProcessNum)
        {
            return await m_workForceRepository.GetExportXmlFile(xmlProcessNum);
        }
        public async Task<DataTable> GetExportTrainingData()
        {
            return await m_workForceRepository.GetExportTrainingData();
        }

        // Author       : Shaheena Shaik
        // Creation date:4-July-2017
        /// <summary>
        /// Validating PayCurrency whether it is already exists in database or not
        /// </summary>
        /// <param name="payCurrencyCode">The currencyCode which is newly added</param>
        /// <returns></returns>
        public async Task<string> payCurrencyValidation(string payCurrencyCode)
        {
            return await m_workForceRepository.payCurrencyValidation(payCurrencyCode);
        }



        #region Excel

        #region Variable Declaration

        private OleDbConnection cn;
        private OleDbDataAdapter daAdapter;

        //private string ExcelCon = @"Provider=Microsoft.Jet.OLEDB.4.0;";
        private string strConnectionString;

        //private string strParseError = "";
        private string SheetName, Range;

        #endregion

        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// Initialize ExcelXML component using the sepecifed File name, By default HDR property will be false.
        /// </summary>
        /// <param name="strFileName"></param>
        public void InitializeConnection(string filePath)
        {
            strConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + (filePath) + ";Extended Properties=\"Excel 12.0;HDR=YES;\"";
            cn = new OleDbConnection(strConnectionString);
        }

        #region Excel File Info

        public String[] GetExcelSheetNames()
        {
            System.Data.DataTable dt = null;
            try
            {
                cn.Open();
                // Get the data table containing the schema
                dt = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                if (dt == null)
                {
                    return null;
                }

                String[] excelSheets = new String[dt.Rows.Count];
                int i = 0;
                // Add the sheet name to the string array.
                foreach (DataRow row in dt.Rows)
                {
                    string strSheetTableName = row["TABLE_NAME"].ToString();
                    excelSheets[i] = strSheetTableName.Replace("'", "").Split('$')[0];
                    break;
                }
                return excelSheets;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                // Clean up.
                cn.Close();
            }
        }

        public DataTable GetDataTable(string strSheetName)
        {
            try
            {
                string strComand;
                if (strSheetName.IndexOf("|") > 0)
                {
                    SheetName = strSheetName.Substring(0, strSheetName.IndexOf("|"));
                    Range = strSheetName.Substring(strSheetName.IndexOf("|") + 1);
                    strComand = "select * from [" + SheetName + "$" + Range + "]";
                }
                else
                {
                    strComand = "select * from [" + strSheetName + "$]";
                }
                daAdapter = new OleDbDataAdapter(strComand, cn);
                DataTable dt = new DataTable("Table");
                daAdapter.FillSchema(dt, SchemaType.Source);
                daAdapter.Fill(dt);
                cn.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
        #endregion
    }
}
