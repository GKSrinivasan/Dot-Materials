// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   ICommentProcessManager
// Description    :   Interface signature for CommentProcessManager
// Author         :   Raja Ganapathy		
// Creation Date  :   20-06-2016

using Laserbeam.BusinessObject.Common;
using Laserbeam.Constant.HR;
using System.Linq;
using System.Threading.Tasks;

namespace Laserbeam.ProcessManager.Interfaces.Common
{
    public interface ICommentProcessManager
    {
        IQueryable<CommentModel> GetComments(int commentKey, CommentType commentType,int userNum,int loggedSelectedUserNum);
        void PutUpdateComments(CommentInputModel comment, CommentType type, bool isEditItem);
        Task DeleteComments(int commentKey);
        CompensationTypeConfiguration GetRuleConfiguration();
        Task<int> UpdateCommentStatus(int empJobNum, int userNum);
    }
}
