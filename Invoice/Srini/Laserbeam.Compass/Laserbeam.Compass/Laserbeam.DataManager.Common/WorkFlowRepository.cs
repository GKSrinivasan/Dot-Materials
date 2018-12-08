// Copyright (c) 2016 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
// Component Name  :  WorkFlowRepository
// Description     :  Repository for WorkFlow
// Author         :  Muthuvel Sabarish M	
// Creation Date  :  27-Mar-2017 

using Laserbeam.BusinessObject.Common;
using Laserbeam.BusinessObject.Common.Constants;
using Laserbeam.Constant.HR;
using Laserbeam.DataManager.Interfaces.Common;
using Laserbeam.DataManager.Interfaces.Core;
using Laserbeam.EntityManager.Common;
using Laserbeam.Libraries.Core.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Laserbeam.DataManager.Common
{
    public class WorkFlowRepository : IWorkFlowRepository
    {
        #region Fields
        // Author         :   Muthuvel Sabarish M	
        // Creation Date  :   27-Mar-2017 
        /// <summary>
        /// Instance of BaseRepository
        /// </summary>
        private IBaseRepository m_baseRepository;
        private ITenantCacheProvider m_tenantCacheProvider;
        #endregion

        #region Constructor
        // Author         :   Muthuvel Sabarish M	
        // Creation Date  :   27-Mar-2017 
        /// <summary>
        /// Constructor to create an instance of ConfigurationRepository
        /// </summary>
        /// <param name="baseRepository">Instance of BaseRepository</param>
        public WorkFlowRepository(IBaseRepository baseRepository, ITenantCacheProvider tenantCacheProvider)
        {
            m_baseRepository = baseRepository;
            m_tenantCacheProvider = tenantCacheProvider;
        }

        #endregion

        #region Public
        // Author         :   Muthuvel Sabarish M	
        // Creation Date  :   28-Mar-2017 
        /// <summary>
        /// Gets the data to bind to process DropDown
        /// </summary>
        /// <returns>Returns process</returns>
        public IQueryable<Module> GetProcessList()
        {
            return m_baseRepository.GetQuery<Module>().Where(z => z.Active == true);
        }

        public bool GetWorkFlowDataIsCustomized()
        {
            //bool IsDataCustomized = ((m_baseRepository.GetQuery<BusSetting>().Where(s => s.KeyValue == "WorkFlowMode" && s.KeyId== "FeatureConfiguration").Select(a => a.KeyDataValue).FirstOrDefault()).Equals("Custom")) ? true : false; ;
            bool IsDataCustomized = ((m_tenantCacheProvider.GetBusinessSetting().FeatureConfiguration.WorkFlowMode.Equals("Custom")) ? true : false); 
            return IsDataCustomized;
        }

        public string GetWorkFlowLevel()
        {
            //return m_baseRepository.GetQuery<BusSetting>(s => s.KeyId == "FeatureConfiguration" && s.KeyValue == "WorkFlowLevel").Select(a => a.KeyDataValue).FirstOrDefault();
            return m_tenantCacheProvider.GetBusinessSetting().FeatureConfiguration.WorkFlowLevel.ToString();

        }

        public int GetDefaultProcess()
        {
            return m_baseRepository.GetQuery<Module>().Where(z => z.ModuleKey == ModuleConstants.Compensation && z.Active == true).Select(x => x.ModuleNum).FirstOrDefault();
        }

        // Author         :   Muthuvel Sabarish M	
        // Creation Date  :   28-Mar-2017 
        /// <summary>
        /// Gets the data to bind to workflow grid
        /// </summary>
        /// <returns>Returns workflow data</returns>
        public async Task<IEnumerable<WorkFlowGrid>> GetGridDataValues(int processTypeNum, int meritCycleYear)
        {
            SqlParameter[] parameters = {
                                  new SqlParameter("@ModuleNum",processTypeNum),
                                  };
            var workFlowGrid = await m_baseRepository.GetData<WorkFlowGrid>("[Common].[USP_WP_GET_WorkFlowApprovalData] @ModuleNum", parameters);
            return workFlowGrid;
        }

        // Author         :   Muthuvel Sabarish M	
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// Update the data in Workflow grid
        /// </summary>
        /// <returns>Update workflow data</returns>
        public async Task<bool> UpdateWorkFlowGrid(List<WorkFlowGrid> workFlow, int processNum, int year, int userNum)
        {
            bool isUpdateSuccess;
            var employee = (from emp in m_baseRepository.GetQuery<Employee>()
                            join empjob in m_baseRepository.GetQuery<EmployeeJob>() on emp.EmployeeNum equals empjob.EmployeeNum
                            where empjob.JobYear == year && emp.EmployeeID != "999999999" && empjob.JobStatus !="T" && empjob.JobStatus != "N" 
                            select new { empjob.EmpJobNum, emp.EmployeeName, emp.MyHRID, emp.EmployeeNum, empjob.JobYear });

            var empComp = (from wrk in workFlow
                           join ee in employee on wrk.EmployeeID equals ee.MyHRID
                           join empcomptable in m_baseRepository.GetQuery<EmployeeCompApprovalLevel>() on ee.EmpJobNum equals empcomptable.EmpJobNum
                           where empcomptable.ModuleNum == processNum && empcomptable.EmpJobNum == ee.EmpJobNum
                           //select new {a =1});
                           select new EmployeeCompApprovalLevel
                           {
                               AppMgrOneEmpNum = (from ee1 in employee
                                                  where wrk.EmpID_1 == ee1.MyHRID
                                                  select (int?)ee1.EmpJobNum).FirstOrDefault(),
                               AppMgrTwoEmpNum = ((from ee1 in employee
                                                   where wrk.EmpID_2 == ee1.MyHRID
                                                   select (int?)ee1.EmpJobNum).FirstOrDefault()),

                               AppMgrThreeEmpNum = ((from ee1 in employee
                                                     where wrk.EmpID_3 == ee1.MyHRID
                                                     select (int?)ee1.EmpJobNum).FirstOrDefault()),

                               AppMgrFourEmpNum = ((from ee1 in employee
                                                    where wrk.EmpID_4 == ee1.MyHRID
                                                    select (int?)ee1.EmpJobNum).FirstOrDefault()),
                               AppMgrFiveEmpNum = ((from ee1 in employee
                                                    where wrk.EmpID_5 == ee1.MyHRID
                                                    select (int?)ee1.EmpJobNum).FirstOrDefault()),
                               AppMgrSixEmpNum = ((from ee1 in employee
                                                   where wrk.EmpID_6 == ee1.MyHRID
                                                   select (int?)ee1.EmpJobNum).FirstOrDefault()),
                               AppMgrSevenEmpNum = ((from ee1 in employee
                                                     where wrk.EmpID_7 == ee1.MyHRID
                                                     select (int?)ee1.EmpJobNum).FirstOrDefault()),
                               AppMgrEightEmpNum = ((from ee1 in employee
                                                     where wrk.EmpID_8 == ee1.MyHRID
                                                     select (int?)ee1.EmpJobNum).FirstOrDefault()),
                               AppMgrNineEmpNum = ((from ee1 in employee
                                                    where wrk.EmpID_9 == ee1.MyHRID
                                                    select (int?)ee1.EmpJobNum).FirstOrDefault()),
                               AppMgrTenEmpNum = ((from ee1 in employee
                                                   where wrk.EmpID_10 == ee1.MyHRID
                                                   select (int?)ee1.EmpJobNum).FirstOrDefault()),

                               EmpJobNum = empcomptable.EmpJobNum,
                               ModuleNum = processNum,
                               EmployeeCompApprovalLevelNum = empcomptable.EmployeeCompApprovalLevelNum,
                               UpdatedBy = userNum,
                               UpdatedDate = DateTime.Now


                           }).ToList();
            try
            {

                foreach (var item in empComp)
                {
                    EmployeeCompApprovalLevel emptable = GetEmployeeCompApprovalLevel(item.EmployeeCompApprovalLevelNum);

                    emptable.AppMgrOneEmpNum = item.AppMgrOneEmpNum;
                    emptable.AppMgrTwoEmpNum = item.AppMgrTwoEmpNum;
                    emptable.AppMgrThreeEmpNum = item.AppMgrThreeEmpNum;
                    emptable.AppMgrFourEmpNum = item.AppMgrFourEmpNum;
                    emptable.AppMgrFiveEmpNum = item.AppMgrFiveEmpNum;
                    emptable.AppMgrSixEmpNum = item.AppMgrSixEmpNum;
                    emptable.AppMgrSevenEmpNum = item.AppMgrSevenEmpNum;
                    emptable.AppMgrEightEmpNum = item.AppMgrEightEmpNum;
                    emptable.AppMgrNineEmpNum = item.AppMgrNineEmpNum;
                    emptable.AppMgrTenEmpNum = item.AppMgrTenEmpNum;
                    emptable.EmpJobNum = item.EmpJobNum;
                    emptable.ModuleNum = item.ModuleNum;
                    emptable.UpdatedBy = item.UpdatedBy;
                    emptable.UpdatedDate = DateTime.Now;
                    m_baseRepository.Edit<EmployeeCompApprovalLevel>(emptable);
                }

                List<EmployeeWorkFlowChanx> changedEmployees = empComp.Select(x => new EmployeeWorkFlowChanx
                {
                    EmpJobNum = x.EmpJobNum,
                    UserNum = userNum
                }).ToList();
                m_baseRepository.Add<EmployeeWorkFlowChanx>(changedEmployees);
                m_baseRepository.SaveChanges();

                SqlParameter[] parameters = {
                                  new SqlParameter("@UserNum", userNum),
                                  new SqlParameter("@ModuleNum",processNum),
                                  };
                await  m_baseRepository.ExecuteStoredProcedure("[Common].[USP_WP_PUT_ReloadPivotedEmployeeApprovalLevel]", parameters);

                if (empComp.Count != 0)
                {
                    isUpdateSuccess = true;
                }
                else { isUpdateSuccess = false; }
                BusSetting busSetting = m_baseRepository.GetQuery<BusSetting>(s => s.KeyValue == "WorkFlowMode" && s.KeyId== "FeatureConfiguration").FirstOrDefault();
                busSetting.KeyDataValue = "Custom";
                m_baseRepository.Edit<BusSetting>(busSetting);
                m_baseRepository.SaveChanges();
                m_tenantCacheProvider.RemoveCache(ApplicationCacheConstants.BussinessSetting);
            }
            catch// (Exception ex)
            {
                isUpdateSuccess = false;
            }
            return isUpdateSuccess;

        }



        public List<string> GetStatusNotIn()
        {
            List<string> status = new List<string>();
            status.Add("T");
            status.Add("N");
            status.Add("I");
            return status;
        }

        // Author         :   Muthuvel Sabarish M	
        // Creation Date  :   31-Mar-2017 
        /// <summary>
        /// Complete Update/Delete data in workflow grid based on action status 
        /// </summary>
        /// <returns>Update/Delete data in workflow grid </returns>
        public async Task<bool> CopyInWorkFlowGrid(int meritCycleYear, string headerID, string headerValue, string FilteredEmployee, int ProcessNum, string actionStatus, int userNum)
        {
            bool isUpdateSuccess;
            if (actionStatus == "Update")
            {
                try
                {
                    if (headerID != null && headerValue != null)
                    {
                        SqlParameter[] parameters = {
                                  new SqlParameter("@EmployeeId", headerValue),
                                  new SqlParameter("@ColumnName",headerID),
                                  new SqlParameter("@Year",meritCycleYear),     
                                  new SqlParameter("@ProcessNum",ProcessNum),
                                  new SqlParameter("@FilteredEmployee",FilteredEmployee),
                                            };
                        await m_baseRepository.ExecuteStoredProcedure("[Common].[USP_WP_PUT_ApprovalData]", parameters);
                    }
                    isUpdateSuccess = true;
                    BusSetting busSetting = m_baseRepository.GetQuery<BusSetting>(s => s.KeyValue == "WorkFlowMode" && s.KeyId == "FeatureConfiguration").FirstOrDefault();
                    busSetting.KeyDataValue = "Custom";
                    m_baseRepository.Edit<BusSetting>(busSetting);
                    m_baseRepository.SaveChanges();
                    m_tenantCacheProvider.RemoveCache(ApplicationCacheConstants.BussinessSetting);
                }
                catch// (Exception ex)
                {
                    isUpdateSuccess = false;
                }
                return isUpdateSuccess;
            }
            else
            {
                try
                {
                    if (headerID != null)
                    {
                        SqlParameter[] parameters = {
                                  new SqlParameter("@ColumnName",headerID),
                                  new SqlParameter("@Year",meritCycleYear),     
                                  new SqlParameter("@ProcessNum",ProcessNum),
                                  new SqlParameter("@FilteredEmployee",FilteredEmployee),
                                            };
                        await m_baseRepository.ExecuteStoredProcedure("[Common].[USP_WP_Delete_EmployeeCompApproval]", parameters);
                    }
                    isUpdateSuccess = true;
                    BusSetting busSetting = m_baseRepository.GetQuery<BusSetting>(s => s.KeyValue == "WorkFlowMode" && s.KeyId == "FeatureConfiguration").FirstOrDefault();
                    busSetting.KeyDataValue = "Custom";
                    m_baseRepository.Edit<BusSetting>(busSetting);
                    m_baseRepository.SaveChanges();
                    m_tenantCacheProvider.RemoveCache(ApplicationCacheConstants.BussinessSetting);
                }
                catch// (Exception ex)
                {
                    isUpdateSuccess = false;
                }
                return isUpdateSuccess;
            }
            //SqlParameter[] param = {
            //                      new SqlParameter("@UserNum", userNum),
            //                      new SqlParameter("@ModuleNum",ProcessNum),
            //                      };
            //await m_baseRepository.ExecuteStoredProcedure("[Common].[USP_WP_PUT_ReloadPivotedEmployeeApprovalLevel]", param);
        }

        // Author         :   Muthuvel Sabarish M	
        // Creation Date  :   31-Mar-2017 
        /// <summary>
        /// Get the employeeid 
        /// </summary>
        /// <returns>Returns employeeid</returns>
        public List<EmployeeModel> GetEmployeeIdList(int year)
        {

            List<EmployeeModel> id = m_baseRepository.GetQuery<EmployeeJob>(new string[] { "Employee" }, x => x.JobYear == year && (x.JobStatus == "A" || x.JobStatus == "L")).Select(x => new EmployeeModel { EmployeeID = x.Employee.MyHRID }).ToList();           
            return id;
        }

        // Author         :   Muthuvel Sabarish M	
        // Creation Date  :   3-Mar-2017 
        /// <summary>
        /// Get the Workflow data based on org structure 
        /// </summary>
        /// <returns>Returns Workflow data based on org structure </returns>
        public async Task<int> ReloadGridDataValues(int processNum, int userNum,int Level,bool LevelBasedWFData)
        {
            int result=0;            
            if(!LevelBasedWFData)
            {
                SqlParameter[] parameters ={
                                          new SqlParameter("@FlowProcessID",processNum),
                                          new SqlParameter("@UserID",userNum)
                                      };
                result = await m_baseRepository.ExecuteStoredProcedure("[Common].[USP_WP_GET_WorkFlowProcess]", parameters);

                BusSetting data=  m_baseRepository.GetQuery<BusSetting>(s => s.KeyId == "FeatureConfiguration" && s.KeyValue == "WorkFlowLevel").FirstOrDefault();
                data.KeyDataValue = 0.ToString();
                m_baseRepository.Edit<BusSetting>(data);
                m_baseRepository.SaveChanges();
                m_tenantCacheProvider.RemoveCache(ApplicationCacheConstants.BussinessSetting);
            }

            if(LevelBasedWFData && Level != 0)
            {
                SqlParameter[] parameter = {
                                                new SqlParameter ("@Level",Level),new SqlParameter("@FlowProcessID",processNum),
                                          new SqlParameter("@UserID",userNum)
                };
                result = await m_baseRepository.ExecuteStoredProcedure("[Talent].[USP_WP_PUT_WorkFlowLevel]", parameter);
            }
            //SqlParameter[] param = {
            //                      new SqlParameter("@UserNum", userNum),
            //                      new SqlParameter("@ModuleNum",processNum),
            //                      };
            //await m_baseRepository.ExecuteStoredProcedure("[Common].[USP_WP_PUT_ReloadPivotedEmployeeApprovalLevel]", param);

            return result;
        }


        

        // Author       : Shaheena Shaik
        // Creation Date:28-March-2017
        // Reviewed By  :Raja Ganapathy
        // Reviewed Date: 6-April-2017
        /// <summary>
        /// GettingWorkflowExport Details from database
        /// </summary>
        /// <param name="moduleNum">Module Num of type Compensation</param>
        /// <returns>Returning a Datatable of WorkFlow Export Values</returns>
        public async Task<DataTable> GetWorkFlowExportDetails(int moduleNum)
        {
            SqlParameter[] parameters = {                                  
                                  new SqlParameter("@moduleNum",moduleNum)                                  
                            };
            var workFlowExportTableData = await m_baseRepository.GetDataTableFromStoredProcedure("[Common].[USP_WP_GET_ApprovalDataExport]", parameters);
            return workFlowExportTableData;
        }

        public async Task<DataTable> GetWorkFlowOrgChart(int year)
        {
            SqlParameter[] parameters = {
                                  new SqlParameter("@JobYear",year)
                            };
            var workFlowExportTableData = await m_baseRepository.GetDataTableFromStoredProcedure("[Talent].[USP_GET_WF_ManagerOrgChart]", parameters);
            return workFlowExportTableData;
        }

        public async Task<int> DeleteApprovalTemplateData(int xmlProcessNum)
        {
            SqlParameter[] parameters = { new SqlParameter("@XmlProcessNum", xmlProcessNum), new SqlParameter("@TemplateName", "ApprovalData") };
            return await m_baseRepository.ExecuteStoredProcedure("[Talent].[USP_SLP_DeleteLoadedSheetDetails]", parameters);
        }

        public async Task<IEnumerable<TemplateDataModel>> GetApprovalGridData(int userNum)
        {
            SqlParameter[] parameters = { new SqlParameter("@UserNum", userNum), new SqlParameter("@TemplateName", "ApprovalData") };
            return await m_baseRepository.GetData<TemplateDataModel>("[Talent].[USP_SLP_GET_LoadedSheetDetails] @UserNum,@TemplateName", parameters);
        }

        public async Task<DataTable> GetApprovalDataTemplate()
        {
            return await m_baseRepository.GetDataTableFromStoredProcedure("[Talent].[USP_SLP_GET_ApprovalDataTemplate]");
        }

        public int UpdateXmlProcess(string sheetName, string fileSourcePath, int recordCount,int userNum)
        {
            XmlProcess xmlprocess = new XmlProcess();
            xmlprocess.ProcessDate = DateTime.Now;
            xmlprocess.RecordCount = recordCount;
            xmlprocess.XmlFilename = sheetName;
            xmlprocess.SheetName = sheetName;
            xmlprocess.FileSourcePath = fileSourcePath;
            xmlprocess.ValidateStatus = 0;
            xmlprocess.ProcessStatus = 0;
            xmlprocess.UserNum = userNum;
            xmlprocess.MetaXmlTemplateNum = m_baseRepository.GetQuery<MetaXmlTemplate>(x => x.TemplateName == "ApprovalData").Select(x => x.MetaXmlTemplateNum).FirstOrDefault();
            m_baseRepository.Add<XmlProcess>(xmlprocess);
            m_baseRepository.SaveChanges();
            return xmlprocess.XmlProcessNum;
        }

        public async Task<bool> InsertStagingTableData(DataTable userData, string DbName, List<string> columnsName, int xmlProcessNum)
        {
            bool result = await m_baseRepository.SqlBulkInsert(userData, DbName, columnsName) != "" ? false : true;
            SqlParameter[] parameters = { new SqlParameter("@XmlProcessNum", xmlProcessNum),
                                        new SqlParameter("@TableName", "Talent.StagingApprovalData") };
            await m_baseRepository.ExecuteStoredProcedure("[Talent].[USP_SLP_PUT_XMLProcessNum]", parameters);
            return result;
        }


        public async Task<int> ValidateApprovalData()
        {
            var result = await m_baseRepository.GetData<int>("[Talent].[USP_SLP_PUT_ApprovalDataValidation]", new SqlParameter[] { });
            return result.FirstOrDefault();
        }

        public async Task<int> ProcessApprovalData()
        {
            var result = await m_baseRepository.GetData<int>("[Talent].[USP_SLP_PUT_ImportApprovalData]", new SqlParameter[] { });
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<TemplateErrorListModel>> GetApprovalDataErrorList()
        {
            return await m_baseRepository.GetData<TemplateErrorListModel>("[Talent].[USP_SLP_GET_ApprovalErrorData]", new SqlParameter[] { });
        }

        public async Task<DataTable> GetApprovalDataErrorExport()
        {
            return await m_baseRepository.GetDataTableFromStoredProcedure("[Talent].[USP_SLP_GET_ApprovalDataErrorExport]");
        }

        public async Task<int> GetWorkFlowDataErrorCount()
        {
            SqlParameter[] parameter = { new SqlParameter("@TableName", "Talent.StagingApprovalData") };
            var result = await m_baseRepository.GetData<int>("[Talent].[USP_SLP_GET_ErrorRecordCount] @TableName", parameter);
            return result.FirstOrDefault();
        }

        public async Task<DataTable> GetExportXmlFile(int xmlProcessNum)
        {
            SqlParameter[] parameters = { new SqlParameter("@XmlProcessNum", xmlProcessNum),
                                        new SqlParameter("@TableName", "Talent.StagingApprovalData") };
            return await m_baseRepository.GetDataTableFromStoredProcedure("[Talent].[USP_SLP_GET_LoadedDataExport]", parameters);
        }



        #endregion

        #region Private

        // Author         :   Muthuvel Sabarish M	
        // Creation Date  :   31-Mar-2017 
        /// <summary>
        /// Get the GetEmployeeCompApprovalLevel data 
        /// </summary>
        /// <returns>Returns GetEmployeeCompApprovalLevel</returns>
        private EmployeeCompApprovalLevel GetEmployeeCompApprovalLevel(int employeeCompApprovalLevelNum)
        {
            return m_baseRepository.GetQuery<EmployeeCompApprovalLevel>(s => s.EmployeeCompApprovalLevelNum == employeeCompApprovalLevelNum).FirstOrDefault();
        }

        #endregion

    }
}



