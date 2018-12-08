// Copyright (c) 2016 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.
// Component Name  :  IPageCustomizationRepository
// Description     :  Interface signature for PageCustomizationRepository
// Author         : Muthuvel Sabarish.M
// Creation Date  :  25-April-2017


using Laserbeam.BusinessObject.Common;
using Laserbeam.EntityManager.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Laserbeam.DataManager.Interfaces.Common
{
    public interface IPageCustomizationRepository 
    {
        List<PageCustomization> getUserCustomizationDetails(string displayVisibile);
        void UpdatePageCustomization(List<MetaColumn> metacolumnDataList, int userNum);
        Task<int> ResetPageCustomization();
    }
}
