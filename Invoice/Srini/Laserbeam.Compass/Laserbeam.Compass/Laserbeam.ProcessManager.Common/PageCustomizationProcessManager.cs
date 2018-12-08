// Copyright (c) 2014 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name :   PageCustomization
// Description    :   All Business logic related to PageCustomization is placed here 	
// Author         :   Muthuvel Sabarish.M
// Creation Date  :   25-April-2017

using Laserbeam.BusinessObject.Common;
using Laserbeam.DataManager.Interfaces.Common;
using Laserbeam.EntityManager.Common;
using Laserbeam.ProcessManager.Interfaces.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laserbeam.ProcessManager.Common
{
    public class PageCustomizationProcessManager : IPageCustomizationProcessManager
    {
        #region Fields
        // Author         : Muthuvel Sabarish.M 
        // Creation Date  :  25-April-2017
        /// <summary>
        /// Instance of  pageCustomizationRepository
        /// </summary>
        private IPageCustomizationRepository m_pageCustomizationRepository;// {get;set;}

        #endregion
        #region Fields
        // Author         :  Muthuvel Sabarish.M 
        // Creation Date  :  25-April-2017
        /// <summary>
        /// Instance of  pageCustomizationRepository
        /// </summary>
        public PageCustomizationProcessManager(IPageCustomizationRepository pageCustomizationRepository)
        {
            m_pageCustomizationRepository = pageCustomizationRepository;
        }

        #endregion


        // Author        :  Muthuvel Sabarish.M
        // Creation Date :  25-April-2017
        // <summary>
        //get the data and bind to grid 
        //</summary>        
        // <param name=""></param>  

        public List<PageCustomization> getUserCustomizationDetails(string displayVisibile)
        {
            return m_pageCustomizationRepository.getUserCustomizationDetails(displayVisibile);
        }



        // Author        :  Muthuvel Sabarish.M
        // Creation Date :  25-April-2017
        // <summary>
        //Reset the data and bind to grid 
        //</summary>        
        // <param name=""></param>  
        public async Task<int> ResetPageCustomization()
        {
            return await m_pageCustomizationRepository.ResetPageCustomization();
        }

        // Author        :  Muthuvel Sabarish.M
        // Creation Date :  25-April-2017
        // <summary>
        //update the data and bind to grid 
        //</summary>        
        // <param name=""></param>  
        public void UpdatePageCustomization(List<PageCustomization> pageCustomizationDetails, int userNum)
        {
           List<MetaColumn> metacolumnDataList =  (from metaColumn in pageCustomizationDetails
                    select new MetaColumn
                    {   
                        GeneralAliasName = metaColumn.AliasName,
                        FunctionalGroup = metaColumn.FunctionalGroup,
                        PopupDisplay = metaColumn.PopupDisplay,
                        GridDisplay = metaColumn.GridDisplay,
                        ExportDisplay = metaColumn.ExportDisplay,
                        FilterDisplay = metaColumn.FilterDisplay,
                        MetaColumnID = metaColumn.MetaColumnID
                    }).ToList();

            m_pageCustomizationRepository.UpdatePageCustomization(metacolumnDataList, userNum);
        }
    }
}
