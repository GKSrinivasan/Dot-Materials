// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  :    PageCustomizationRepository
// Description     : 	Repository for PageCustomization
// Author          :	Muthuvel Sabarish.M
// Creation Date   : 	25-April-2017

using Laserbeam.BusinessObject.Common;
using Laserbeam.DataManager.Interfaces.Common;
using Laserbeam.DataManager.Interfaces.Core;
using Laserbeam.EntityManager.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laserbeam.DataManager.Common
{
    public class PageCustomizationRepository : IPageCustomizationRepository
    {
        #region Fields
        // Author         :   Muthuvel Sabarish.M
        // Creation Date  :   25-April-2017
        /// <summary>
        /// Instance of BaseRepository
        /// </summary>
        private IBaseRepository m_baseRepository;
        #endregion

        #region Constructors
        // Author        :  Muthuvel Sabarish.M
        // Creation Date :  25-April-2017
        /// <summary>
        ///Initializes objects used in this class 
        //</summary>        
        // <param name="baseRepository">Base Repository Object</param>  

        public PageCustomizationRepository(IBaseRepository baseRepository)
        {
            m_baseRepository = baseRepository;
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
            List<PageCustomization> model = new List<PageCustomization>();
            switch (displayVisibile)
            {
                case "Grid":
                    model= (from metaColumn in m_baseRepository.GetQuery<MetaColumn>()
                            where metaColumn.MasterDisplay != false && metaColumn.GridDisplayDefaultValue==true
                            select new PageCustomization
                            {
                                MetaColumnID = metaColumn.MetaColumnID,
                                AliasName = metaColumn.GeneralAliasName,
                                FunctionalGroup = metaColumn.FunctionalGroup,
                                PopupDisplay = metaColumn.PopupDisplay,
                                GridDisplay = metaColumn.GridDisplay,
                                ExportDisplay = metaColumn.ExportDisplay,
                                FilterDisplay = metaColumn.FilterDisplay,
                                ColumnName = metaColumn.FieldName

                            }).ToList();
                    break;
                case "Filter":
                    model= (from metaColumn in m_baseRepository.GetQuery<MetaColumn>()
                            where metaColumn.MasterDisplay != false && metaColumn.FilterDisplayDefaultValue == true
                            select new PageCustomization
                            {
                                MetaColumnID = metaColumn.MetaColumnID,
                                AliasName = metaColumn.GeneralAliasName,
                                FunctionalGroup = metaColumn.FunctionalGroup,
                                PopupDisplay = metaColumn.PopupDisplay,
                                GridDisplay = metaColumn.GridDisplay,
                                ExportDisplay = metaColumn.ExportDisplay,
                                FilterDisplay = metaColumn.FilterDisplay,
                                ColumnName = metaColumn.FieldName

                            }).ToList();
                    break;
                case "Popup":
                    model= (from metaColumn in m_baseRepository.GetQuery<MetaColumn>()
                            where metaColumn.MasterDisplay != false && metaColumn.PopupDisplayDefaultValue == true
                            select new PageCustomization
                            {
                                MetaColumnID = metaColumn.MetaColumnID,
                                AliasName = metaColumn.GeneralAliasName,
                                FunctionalGroup = metaColumn.FunctionalGroup,
                                PopupDisplay = metaColumn.PopupDisplay,
                                GridDisplay = metaColumn.GridDisplay,
                                ExportDisplay = metaColumn.ExportDisplay,
                                FilterDisplay = metaColumn.FilterDisplay,
                                ColumnName = metaColumn.FieldName

                            }).ToList();
                    break;
                case "Export":
                    model= (from metaColumn in m_baseRepository.GetQuery<MetaColumn>()
                            where metaColumn.MasterDisplay != false && metaColumn.ExportDisplayDefaultValue == true
                            select new PageCustomization
                            {
                                MetaColumnID = metaColumn.MetaColumnID,
                                AliasName = metaColumn.GeneralAliasName,
                                FunctionalGroup = metaColumn.FunctionalGroup,
                                PopupDisplay = metaColumn.PopupDisplay,
                                GridDisplay = metaColumn.GridDisplay,
                                ExportDisplay = metaColumn.ExportDisplay,
                                FilterDisplay = metaColumn.FilterDisplay,
                                ColumnName = metaColumn.FieldName

                            }).ToList();
                    break;
            }
            return model;
        }

        // Author        :  Muthuvel Sabarish.M
        // Creation Date :  25-April-2017
        // <summary>
        //update the data and bind to grid 
        //</summary>        
        // <param name=""></param>  

        public void UpdatePageCustomization(List<MetaColumn> metacolumnDataList, int userNum)
        {
            if (metacolumnDataList != null && metacolumnDataList.Count > 0)
                foreach (var item in metacolumnDataList)
                {
                    var metaColumnData = m_baseRepository.GetQuery<MetaColumn>(x => x.MetaColumnID == item.MetaColumnID).FirstOrDefault();
                    metaColumnData.ExportDisplay = item.ExportDisplay;
                    metaColumnData.FilterDisplay = item.FilterDisplay;
                    metaColumnData.GridDisplay = item.GridDisplay;
                    metaColumnData.PopupDisplay = item.PopupDisplay;
                    metaColumnData.GeneralAliasName = item.GeneralAliasName;
                    metaColumnData.UpdatedDate = DateTime.Now;
                    metaColumnData.UpdatedBy = userNum;
                    m_baseRepository.Edit(metaColumnData);
                }
            m_baseRepository.SaveChanges();
        }


        // Author        :  Muthuvel Sabarish.M
        // Creation Date :  25-April-2017
        // <summary>
        //Reset the data and bind to grid 
        //</summary>        
        // <param name=""></param>  
        public async Task<int> ResetPageCustomization()
        {
          return  await  m_baseRepository.ExecuteStoredProcedure("[Common].[USP_PC_PUT_ResetDefaultValues]");           
         }
    }
}
