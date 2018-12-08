// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   IPageCustomizationProcessManager
// Description    :   Interface signature for PageCustomizationProcessManager
// Author         :   Muthuvel Sabarish.M
// Creation Date  :    25-April-2017
using Laserbeam.BusinessObject.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Laserbeam.ProcessManager.Interfaces.Common
{
    public interface IPageCustomizationProcessManager
    {
        List<PageCustomization> getUserCustomizationDetails(string displayVisibile);
        void UpdatePageCustomization(List<PageCustomization> pageCustomizationDetails, int userNum);
        Task<int> ResetPageCustomization();
    }
}
