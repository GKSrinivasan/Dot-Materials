// Copyright (c) 2016 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
// Component Name  : 	Comment Business Manager
// Description     : 	Contains Comment related logics	
// Author          :  Raja Ganapathy		
// Creation Date   :  20-06-2016
using Laserbeam.BusinessObject.Common;
using Laserbeam.Constant.HR;
using Laserbeam.ProcessManager.Interfaces.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using Laserbeam.EntityManager.Common;
using Laserbeam.DataManager.Interfaces.Common;
using System.Threading.Tasks;
using Laserbeam.BusinessObject.Common.CachedModels;

namespace Laserbeam.ProcessManager.Common
{
    public class CommentProcessManager : ICommentProcessManager
    {


        #region Fields        
        private ICompensationRepository m_compensationRepository;                
        #endregion

        #region Constructors
        // Author        :  Raja Ganapathy		
        // Creation Date :  20-06-2016
        // Reviewed By    :  Srinivasan kamalakannan
        // Reviewed Date  :  31-08-2016  
        /// <summary>
        /// Initializes objects used in this class
        /// </summary>
        /// <param name="compensationRepository">compensationRepository objects</param>        
        public CommentProcessManager(ICompensationRepository compensationRepository)
        {
            m_compensationRepository = compensationRepository;                                   
        }
        #endregion

        #region Implements

        public async Task<int> UpdateCommentStatus(int empJobNum, int userNum)
        {
            return await m_compensationRepository.UpdateCommentStatus(empJobNum, userNum);
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  20-06-2016
        // Reviewed By    :  Srinivasan kamalakannan
        // Reviewed Date  :  31-08-2016  
        /// <summary>
        /// This method gets all the necessary comments based on the comment type from database 
        /// </summary>
        /// <param name="commentKey">Key based on which comment is retrieved</param>
        /// <param name="commentType">Get the commenttype</param>
        /// <returns>CommentModel object</returns>
        public IQueryable<CommentModel> GetComments(int empJobNum, CommentType commentType,int userNum, int loggedSelectedUserNum)
        {
            
            IQueryable<CommentModel> comments;
            CompensationTypeConfiguration ruleConfiguration = new CompensationTypeConfiguration();
            BusinessSettingModel busSettings =m_compensationRepository.GetCompensationTypeConfiguration();
            ruleConfiguration.DateFormat = busSettings.DateConfiguration.DateFormat;
            if (commentType == CommentType.Compensation)
            {
               
                var compensationTypes = m_compensationRepository.GetCompensationTypes().Where(x => x.CompensationTypeShortName == "Merit" || x.CompensationTypeShortName == "Adjustment" || x.CompensationTypeShortName == "Promotion").ToList();
                var compTypeNums = compensationTypes.Select(x => x.CompensationTypeNum);
                var meritTypeNum = compensationTypes.Where(x => x.CompensationTypeShortName == "Merit").Select(x => x.CompensationTypeNum).FirstOrDefault();
                var adjustmentTypeNum = compensationTypes.Where(x => x.CompensationTypeShortName == "Adjustment").Select(x => x.CompensationTypeNum).FirstOrDefault();
                var promotionTypeNum = compensationTypes.Where(x => x.CompensationTypeShortName == "Promotion").Select(x => x.CompensationTypeNum).FirstOrDefault();
                var bonusTypeNum = compensationTypes.Where(x => x.CompensationTypeShortName == "Bonus").Select(x => x.CompensationTypeNum).FirstOrDefault();              
               IQueryable<CommentModel> compComment= m_compensationRepository.GetCompComments(empJobNum)
                    .Where(x => compTypeNums.Contains(x.CompensationTypeNum) || x.CompensationTypeNum==0)
                    .Select(x => new CommentModel
                    {
                        EmpCommentNum = x.EmployeeCompCommentsNum,
                        EmployeeName = x.AppUser.UserName,
                        FirstName=x.AppUser.FirstName,
                        LastName=x.AppUser.LastName,
                        CommentCreatedDate = x.CreatedDate,
                        CommentUpdatedDate = x.UpdatedDate,
                        Comment = x.Comments,
                       CreatedByUserNumOrEmpNum = x.CreatedBy,
                        UpdatedByUserNumOrEmpNum=x.UpdatedBy,
                        CompensationTypeNum = x.CompensationTypeNum,
                        IsExceedMeritGuideline = (x.CompensationTypeNum == meritTypeNum) ? true : false,
                        IsExceedBonusGuideline = (x.CompensationTypeNum == bonusTypeNum) ? true : false,                       
                        IsExceedAdjustmentGuideline = (x.CompensationTypeNum == adjustmentTypeNum) ? true : false,
                        IsPromotionComments = (x.CompensationTypeNum == promotionTypeNum) ? true : false,
                        IsGeneralComments = (x.EmployeeCompCommentsNum!=null) ? true : false,
                        IsWorkFlowComments = false,
                        Label = (x.CompensationTypeNum == meritTypeNum)?"Merit":(x.CompensationTypeNum== adjustmentTypeNum)?"Adjustment":(x.CompensationTypeNum== promotionTypeNum)?"Promotion":"General",
                        DateFormat = ruleConfiguration.DateFormat
                    });
               IQueryable<CommentModel> workflow=m_compensationRepository.GetWorkFlowComments(empJobNum)                  
                  .Select(x => new CommentModel
                  {
                      EmpCommentNum = x.EmployeeApprovalDetailNum,
                      EmployeeName = x.AppUser.UserName,
                      FirstName = x.AppUser.FirstName,
                      LastName = x.AppUser.LastName,
                      CommentCreatedDate=x.UpdatedDate,
                      CommentUpdatedDate = x.UpdatedDate,
                      Comment = x.Comments,
                      CreatedByUserNumOrEmpNum = x.UpdatedBy,
                      UpdatedByUserNumOrEmpNum = x.UpdatedBy,  
                      CompensationTypeNum=(int)x.ApprovalStatus,                      
                      IsExceedMeritGuideline = false,
                      IsExceedBonusGuideline = false,                      
                      IsExceedAdjustmentGuideline = false,
                      IsPromotionComments = false,
                      IsGeneralComments = (x.EmployeeApprovalDetailNum!=null) ? true:false,
                      IsWorkFlowComments = true,
                      Label = "Workflow",
                      DateFormat = ruleConfiguration.DateFormat
                  });
                return compComment.Union(workflow).OrderByDescending(x => x.CommentUpdatedDate);
              
            }
            else if (commentType == CommentType.CompensationMeritMandit)
            {
                
                int meritTypeNum = m_compensationRepository.getCompensationTypeNum("Merit");
                return m_compensationRepository.GetCompComments(empJobNum)
                    .Where(x => x.CompensationTypeNum == meritTypeNum)
                    .Select(x => new CommentModel
                    {
                        EmpCommentNum = x.EmployeeCompCommentsNum,
                        EmployeeName = x.AppUser.UserName,
                        FirstName = x.AppUser.FirstName,
                        LastName = x.AppUser.LastName,
                        CommentUpdatedDate = x.UpdatedDate,
                        CommentCreatedDate = x.CreatedDate,
                        Comment = x.Comments,
                        UpdatedByUserNumOrEmpNum = x.UpdatedBy,
                        CreatedByUserNumOrEmpNum = x.CreatedBy,
                        CompensationTypeNum = x.CompensationTypeNum,
                        DateFormat = ruleConfiguration.DateFormat
                    }).OrderByDescending(x => x.CommentUpdatedDate);

            }
            else if (commentType == CommentType.Adjustment)
            {
                var adjustmentTypeNum = m_compensationRepository.getCompensationTypeNum("Adjustment");
                return m_compensationRepository.GetCompComments(empJobNum)
                    .Where(x => x.CompensationTypeNum == adjustmentTypeNum)
                    .Select(x => new CommentModel
                    {
                        EmpCommentNum = x.EmployeeCompCommentsNum,
                        EmployeeName = x.AppUser.UserName,
                        FirstName = x.AppUser.FirstName,
                        LastName = x.AppUser.LastName,
                        CommentUpdatedDate = x.UpdatedDate,
                        CommentCreatedDate = x.CreatedDate,
                        Comment = x.Comments,
                        UpdatedByUserNumOrEmpNum = x.UpdatedBy,
                        CreatedByUserNumOrEmpNum = x.CreatedBy,
                        CompensationTypeNum = x.CompensationTypeNum,
                        DateFormat = ruleConfiguration.DateFormat
                    }).OrderByDescending(x => x.CommentUpdatedDate);
            }
            else
            {
                comments = null;
                return comments;
            }
           
        }



        // Author         : Muthuvel Sabarish
        // Creation Date  :  15-Feb-2017
        // Reviewed By    :  Raja Ganapathy		
        // Reviewed Date  :  10-Feb-2017
        /// <summary>
        /// To insert or update the comments
        /// </summary>
        /// <param name="comment">Insert or update comment</param>
        /// <param name="type">Comment type</param>
        /// <param name="isEditItem">Comment is edited or inserted</param>
        // Modified By    : Karthikeyan Shanmugam
        // Modified Date  : 14-Feb-2017
        // Reviewed By    : Hariharasubramaniyan
        // Reviewed Date  : 14-Feb-2017
        // comments       : To show the cilent time based on UTC time it shows depend upon the client location or time zone
        public void PutUpdateComments(CommentInputModel comment, CommentType type, bool isEditItem)
        {            
           var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);
            if (type == CommentType.Compensation)
            {
                if (isEditItem)
                {
                    EmployeeCompComment dataComment = m_compensationRepository.GetCompComments(comment.CommentKey).Where(x=>x.EmployeeCompCommentsNum==comment.EmpCommentNum).First();       
                    dataComment.EmpJobNum = comment.CommentKey;
                    dataComment.Comments = comment.Comment;
                    dataComment.UpdatedBy = comment.CommentedEmployeeNum;
                    dataComment.UpdatedDate = DateTime.Now;
                    dataComment.CompensationTypeNum = 0;
                    m_compensationRepository.PutUpdateCompensationComments(dataComment);

                }
                else
                {
                    EmployeeCompComment dataComment = new EmployeeCompComment();                    
                    dataComment.CreatedBy = comment.CommentedEmployeeNum;
                    dataComment.CreatedDate = DateTime.Now;
                    dataComment.EmpJobNum = comment.CommentKey;
                    dataComment.Comments = comment.Comment;
                    dataComment.CompensationTypeNum = 0; 
                    m_compensationRepository.PutUpdateCompensationComments(dataComment);
                }
            }
           else if (type == CommentType.CompensationMeritMandit)
           {
               string typeKey = "Merit";
               if (isEditItem)
               {
                   EmployeeCompComment dataComment = m_compensationRepository.GetCompComments(comment.CommentKey).Where(x => x.EmployeeCompCommentsNum == comment.EmpCommentNum).First();
                   dataComment.CompensationTypeNum = m_compensationRepository.getCompensationTypeNum(typeKey);
                   dataComment.EmpJobNum = comment.CommentKey;
                   dataComment.Comments = comment.Comment;
                   dataComment.UpdatedBy = comment.CommentedEmployeeNum;
                   dataComment.UpdatedDate = DateTime.Now;
                   m_compensationRepository.PutUpdateCompensationComments(dataComment);
               }
               else
               {
                   EmployeeCompComment dataComment = new EmployeeCompComment();                   
                   dataComment.CreatedBy = comment.CommentedEmployeeNum;
                   dataComment.CreatedDate = DateTime.Now;

                   dataComment.EmpJobNum = comment.CommentKey;
                   dataComment.Comments = comment.Comment;
                   dataComment.CompensationTypeNum = m_compensationRepository.getCompensationTypeNum(typeKey); 
                   m_compensationRepository.PutUpdateCompensationComments(dataComment);
               }
             
           }
            else if (type == CommentType.Promotion)
            {
                string typeKey = "Promotion";
                if (isEditItem)
                {
                    EmployeeCompComment dataComment = m_compensationRepository.GetCompComments(comment.CommentKey).Where(x => x.EmployeeCompCommentsNum == comment.EmpCommentNum).First();
                    dataComment.CompensationTypeNum = m_compensationRepository.getCompensationTypeNum(typeKey);
                    dataComment.EmpJobNum = comment.CommentKey;
                    dataComment.Comments = comment.Comment;
                    dataComment.UpdatedBy = comment.CommentedEmployeeNum;
                    dataComment.UpdatedDate = DateTime.Now;
                    m_compensationRepository.PutUpdateCompensationComments(dataComment);
                }
                else
                {
                    EmployeeCompComment dataComment = new EmployeeCompComment();                    
                    dataComment.CreatedBy = comment.CommentedEmployeeNum;
                    dataComment.CreatedDate = DateTime.Now;
                    dataComment.EmpJobNum = comment.CommentKey;
                    dataComment.Comments = comment.Comment;
                    dataComment.CompensationTypeNum = m_compensationRepository.getCompensationTypeNum(typeKey);
                    m_compensationRepository.PutUpdateCompensationComments(dataComment);
                }
            }
            else if (type == CommentType.Adjustment)
            {
                string typeKey = "Adjustment";
                if (isEditItem)
                {
                    EmployeeCompComment dataComment = m_compensationRepository.GetCompComments(comment.CommentKey).Where(x => x.EmployeeCompCommentsNum == comment.EmpCommentNum).First();
                    dataComment.CompensationTypeNum = m_compensationRepository.getCompensationTypeNum(typeKey);
                    dataComment.EmpJobNum = comment.CommentKey;
                    dataComment.Comments = comment.Comment;
                    dataComment.UpdatedBy = comment.CommentedEmployeeNum;
                    dataComment.UpdatedDate = DateTime.Now;
                    m_compensationRepository.PutUpdateCompensationComments(dataComment);
                }
                else
                {
                    EmployeeCompComment dataComment = new EmployeeCompComment();                    
                    dataComment.CreatedBy = comment.CommentedEmployeeNum;
                    dataComment.CreatedDate = DateTime.Now;
                    dataComment.EmpJobNum = comment.CommentKey;
                    dataComment.Comments = comment.Comment;
                    dataComment.CompensationTypeNum = m_compensationRepository.getCompensationTypeNum(typeKey);
                    m_compensationRepository.PutUpdateCompensationComments(dataComment);
                }
            }
           
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  20-06-2016
        // Reviewed By    :  Srinivasan kamalakannan
        // Reviewed Date  :  31-08-2016  
        /// <summary>
        /// To delete the comments
        /// </summary>
        /// <param name="commentKey">denotes the comment num</param>
        public async Task DeleteComments(int commentKey)
        {
           await m_compensationRepository.deleteComments(commentKey);
        }

        // Author        :  Raja Ganapathy		
        // Creation Date :  20-06-2016
        /// <summary>
        /// To get business configuration for compensation
        /// </summary>
        /// <returns>CompensationTypeConfiguration object</returns>
        public CompensationTypeConfiguration GetRuleConfiguration()
        {
            CompensationTypeConfiguration ruleConfiguration = new CompensationTypeConfiguration();
            BusinessSettingModel busSettings = m_compensationRepository.GetCompensationTypeConfiguration();
            ruleConfiguration.FeatureConfigurationMerit = busSettings.FeatureConfiguration.Merit;
            ruleConfiguration.FeatureConfigurationAdjustment = busSettings.FeatureConfiguration.Adjustment;
            ruleConfiguration.FeatureConfigurationPromotion = busSettings.FeatureConfiguration.Promotion;
            ruleConfiguration.FeatureConfigurationBonus = busSettings.FeatureConfiguration.Bonus;
            ruleConfiguration.FeatureConfigurationWorkFlow = busSettings.FeatureConfiguration.WorkFlow;
            return ruleConfiguration;
        }
        #endregion

        private bool setTrueorFalse(string value)
        {
            if (value != null && value.ToLower().Trim() == "yes")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
