// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  :    ExchangeRateRepository
// Description     : 	Repository for ExchangeRate
// Author          :	Hari.C
// Creation Date   : 	10-Feb-2017 

using Laserbeam.BusinessObject.Common;
using Laserbeam.BusinessObject.Common.Constants;
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
    public class ExchangeRateRepository : IExchangeRateRepository
    {
        #region Fields
        // Author         :   Hariharasubramaniyan Chandrasekaran	
        // Creation Date  :   10-Feb-2017 
        /// <summary>
        /// Instance of BaseRepository
        /// </summary>
        private IBaseRepository m_baseRepository;// {get;set;}
        private ITenantCacheProvider m_tenantCacheProvider;

        #endregion
        #region Fields
        // Author         :   Hariharasubramaniyan Chandrasekaran	
        // Creation Date  :   10-Feb-2017 
        /// <summary>
        /// Instance of BaseRepository
        /// </summary>
        public ExchangeRateRepository(IBaseRepository baseRepository, ITenantCacheProvider tenantCacheProvider)
        {
            m_baseRepository = baseRepository;
            m_tenantCacheProvider = tenantCacheProvider;
        }

        #endregion

       

      public async Task<IEnumerable<ExchangeRateGridData>> GetExchangeRateData()
        {
                 SqlParameter[] parameters = new SqlParameter[] {  };
                 var a = await m_baseRepository.GetData<ExchangeRateGridData>("[Common].[USP_ER_GET_ExchangeRateGridData]", parameters);
            return a;

         
        }

        // Author       : Shaheena Shaik
        // Creation Date: 24-April-2017
        /// <summary>
        /// Getting existing Currency Codes from database
        /// </summary>
        /// <returns>Returning a query of currency codes</returns>
        public async Task<IEnumerable<ExchangeCurrencies>> GetCurrencyCode()
        {

            SqlParameter[] sqlParameter = { };
            var currencies = await m_baseRepository.GetData<ExchangeCurrencies>("[Talent].[USP_Comp_Get_CurrencyData]", sqlParameter);
            return currencies;
            //return m_baseRepository.GetQuery<Currency>(x => x.Active == true).Distinct().Select(m => new CultureCodeData
            //{
            //    CurrencyCode = m.CurrencyCode,
            //    CurrencyCodeNum=m.CurrencyCodeNum

            //});
        }

      public IQueryable<CultureCodeData> GetCultureCode()
      {
          return m_baseRepository.GetQuery<Currency>(x => x.Active == true).Select(m => new CultureCodeData
          {
           CultureCode=m.CultureCode
          });  
      }

        public string GetBaseCurrency()
        {
           //return m_baseRepository.GetQuery<AppSetting>(x => x.AppSettingID == "BaseCurrency").Select(x=>x.AppSettingValue).FirstOrDefault();
            return m_tenantCacheProvider.GetApplicationSetting().BaseCurrency;
        }
      public int AddExchangeRate(ExchangeRateData data)
      {
          int model = 0;
          if(data.CurrencyCodeNum>0)
          {
                Currency currency = m_baseRepository.GetQuery<Currency>(x => x.CurrencyCodeNum == data.CurrencyCodeNum).FirstOrDefault();
                currency.CurrencyCode = data.CurrencyCode;
                currency.CurrencyCodeDescr = data.CurrencyCode;
                currency.Active = true;
                currency.CultureCode = data.CultureCode;
                m_baseRepository.Edit<Currency>(currency);
                ExchangeRate exchangeRate1 = m_baseRepository.GetQuery<ExchangeRate>(x => x.CurrencyCodeNum == data.CurrencyCodeNum).FirstOrDefault();          
                if (exchangeRate1==null)
                {
                    ExchangeRate exchangeRate = new ExchangeRate();
                    exchangeRate.CurrencyCodeNum = data.CurrencyCodeNum;
                    exchangeRate.MeritExchangeRate = data.ExchangeRate;
                    exchangeRate.BonusExchangeRate = data.ExchangeRate;
                    exchangeRate.Active = true;
                    m_baseRepository.Add<ExchangeRate>(exchangeRate);
                }   
                else
                {
                    exchangeRate1.MeritExchangeRate = data.ExchangeRate;
                    exchangeRate1.BonusExchangeRate = data.ExchangeRate;
                    exchangeRate1.Active = true;
                    m_baseRepository.Edit<ExchangeRate>(exchangeRate1);
                }                           
                m_baseRepository.SaveChanges();
            }
          else
          {
              Currency currency = new Currency();
              currency.CurrencyCode = data.CurrencyCode;
              currency.CurrencyCodeDescr = data.CurrencyCode;
              currency.Active = true;
              currency.CultureCode = data.CultureCode;
              m_baseRepository.Add<Currency>(currency);
              ExchangeRate exchangeRate = new ExchangeRate();
              exchangeRate.CurrencyCodeNum = m_baseRepository.GetQuery<Currency>(x => x.CurrencyCode == data.CurrencyCode).Select(x=>x.CurrencyCodeNum).FirstOrDefault();
              exchangeRate.MeritExchangeRate = data.ExchangeRate;
              exchangeRate.BonusExchangeRate = data.ExchangeRate;
              exchangeRate.Active = true;
              m_baseRepository.Add<ExchangeRate>(exchangeRate);
              m_baseRepository.SaveChanges();
          }
          
          return model;
          }

        public int UpdateExchangeRate(Countries exchangeRates,string baseCurrency)
        {
            var a = m_baseRepository.GetQuery<Currency>().ToList();
           List<ExchangeRateListItem> exchangeRateList = new List<ExchangeRateListItem>();
            List<ExchangeRate> result = new List<ExchangeRate>();
            var currency= m_baseRepository.GetQuery<Currency>();
           foreach (var item in a)
           {

                switch (item.CurrencyCode.Trim())
                {

                    case "AED": ExchangeRateListItem AED = new ExchangeRateListItem(); AED.CurrencyCode = "AED"; AED.ExchangeRate = exchangeRates.AED; exchangeRateList.Add(AED); break;
                    case "AMD": ExchangeRateListItem AMD = new ExchangeRateListItem(); AMD.CurrencyCode = "AMD"; AMD.ExchangeRate = exchangeRates.AMD; exchangeRateList.Add(AMD); break;
                    case "ANG": ExchangeRateListItem ANG = new ExchangeRateListItem(); ANG.CurrencyCode = "ANG"; ANG.ExchangeRate = exchangeRates.ANG; exchangeRateList.Add(ANG); break;
                    case "AOA": ExchangeRateListItem AOA = new ExchangeRateListItem(); AOA.CurrencyCode = "AOA"; AOA.ExchangeRate = exchangeRates.AOA; exchangeRateList.Add(AOA); break;
                    case "ARS": ExchangeRateListItem ARS = new ExchangeRateListItem(); ARS.CurrencyCode = "ARS"; ARS.ExchangeRate = exchangeRates.ARS; exchangeRateList.Add(ARS); break;
                    case "AUD": ExchangeRateListItem AUD = new ExchangeRateListItem(); AUD.CurrencyCode = "AUD"; AUD.ExchangeRate = exchangeRates.AUD; exchangeRateList.Add(AUD); break;
                    case "BBD": ExchangeRateListItem BBD = new ExchangeRateListItem(); BBD.CurrencyCode = "BBD"; BBD.ExchangeRate = exchangeRates.BBD; exchangeRateList.Add(BBD); break;
                    case "BDT": ExchangeRateListItem BDT = new ExchangeRateListItem(); BDT.CurrencyCode = "BDT"; BDT.ExchangeRate = exchangeRates.BDT; exchangeRateList.Add(BDT); break;
                    case "BGN": ExchangeRateListItem BGN = new ExchangeRateListItem(); BGN.CurrencyCode = "BGN"; BGN.ExchangeRate = exchangeRates.BGN; exchangeRateList.Add(BGN); break;
                    case "BHD": ExchangeRateListItem BHD = new ExchangeRateListItem(); BHD.CurrencyCode = "BHD"; BHD.ExchangeRate = exchangeRates.BHD; exchangeRateList.Add(BHD); break;
                    case "BRL": ExchangeRateListItem BRL = new ExchangeRateListItem(); BRL.CurrencyCode = "BRL"; BRL.ExchangeRate = exchangeRates.BRL; exchangeRateList.Add(BRL); break;
                    case "BSD": ExchangeRateListItem BSD = new ExchangeRateListItem(); BSD.CurrencyCode = "BSD"; BSD.ExchangeRate = exchangeRates.BSD; exchangeRateList.Add(BSD); break;
                    case "BWP": ExchangeRateListItem BWP = new ExchangeRateListItem(); BWP.CurrencyCode = "BWP"; BWP.ExchangeRate = exchangeRates.BWP; exchangeRateList.Add(BWP); break;
                    case "CAD": ExchangeRateListItem CAD = new ExchangeRateListItem(); CAD.CurrencyCode = "CAD"; CAD.ExchangeRate = exchangeRates.CAD; exchangeRateList.Add(CAD); break;
                    case "CHF": ExchangeRateListItem CHF = new ExchangeRateListItem(); CHF.CurrencyCode = "CHF"; CHF.ExchangeRate = exchangeRates.CHF; exchangeRateList.Add(CHF); break;
                    case "CLP": ExchangeRateListItem CLP = new ExchangeRateListItem(); CLP.CurrencyCode = "CLP"; CLP.ExchangeRate = exchangeRates.CLP; exchangeRateList.Add(CLP); break;
                    case "CNY": ExchangeRateListItem CNY = new ExchangeRateListItem(); CNY.CurrencyCode = "CNY"; CNY.ExchangeRate = exchangeRates.CNY; exchangeRateList.Add(CNY); break;
                    case "COP": ExchangeRateListItem COP = new ExchangeRateListItem(); COP.CurrencyCode = "COP"; COP.ExchangeRate = exchangeRates.COP; exchangeRateList.Add(COP); break;
                    case "CZK": ExchangeRateListItem CZK = new ExchangeRateListItem(); CZK.CurrencyCode = "CZK"; CZK.ExchangeRate = exchangeRates.CZK; exchangeRateList.Add(CZK); break;
                    case "DKK": ExchangeRateListItem DKK = new ExchangeRateListItem(); DKK.CurrencyCode = "DKK"; DKK.ExchangeRate = exchangeRates.DKK; exchangeRateList.Add(DKK); break;
                    case "DOP": ExchangeRateListItem DOP = new ExchangeRateListItem(); DOP.CurrencyCode = "DOP"; DOP.ExchangeRate = exchangeRates.DOP; exchangeRateList.Add(DOP); break;
                    case "EGP": ExchangeRateListItem EGP = new ExchangeRateListItem(); EGP.CurrencyCode = "EGP"; EGP.ExchangeRate = exchangeRates.EGP; exchangeRateList.Add(EGP); break;
                    case "ETB": ExchangeRateListItem ETB = new ExchangeRateListItem(); ETB.CurrencyCode = "ETB"; ETB.ExchangeRate = exchangeRates.ETB; exchangeRateList.Add(ETB); break;
                    case "EUR": ExchangeRateListItem EUR = new ExchangeRateListItem(); EUR.CurrencyCode = "EUR"; EUR.ExchangeRate = exchangeRates.EUR; exchangeRateList.Add(EUR); break;
                    case "FJD": ExchangeRateListItem FJD = new ExchangeRateListItem(); FJD.CurrencyCode = "FJD"; FJD.ExchangeRate = exchangeRates.FJD; exchangeRateList.Add(FJD); break;
                    case "GBP": ExchangeRateListItem GBP = new ExchangeRateListItem(); GBP.CurrencyCode = "GBP"; GBP.ExchangeRate = exchangeRates.GBP; exchangeRateList.Add(GBP); break;
                    case "GHS": ExchangeRateListItem GHS = new ExchangeRateListItem(); GHS.CurrencyCode = "GHS"; GHS.ExchangeRate = exchangeRates.GHS; exchangeRateList.Add(GHS); break;
                    case "GTQ": ExchangeRateListItem GTQ = new ExchangeRateListItem(); GTQ.CurrencyCode = "GTQ"; GTQ.ExchangeRate = exchangeRates.GTQ; exchangeRateList.Add(GTQ); break;
                    case "HKD": ExchangeRateListItem HKD = new ExchangeRateListItem(); HKD.CurrencyCode = "HKD"; HKD.ExchangeRate = exchangeRates.HKD; exchangeRateList.Add(HKD); break;
                    case "HNL": ExchangeRateListItem HNL = new ExchangeRateListItem(); HNL.CurrencyCode = "HNL"; HNL.ExchangeRate = exchangeRates.HNL; exchangeRateList.Add(HNL); break;
                    case "HRK": ExchangeRateListItem HRK = new ExchangeRateListItem(); HRK.CurrencyCode = "HRK"; HRK.ExchangeRate = exchangeRates.HRK; exchangeRateList.Add(HRK); break;
                    case "HUF": ExchangeRateListItem HUF = new ExchangeRateListItem(); HUF.CurrencyCode = "HUF"; HUF.ExchangeRate = exchangeRates.HUF; exchangeRateList.Add(HUF); break;
                    case "IDR": ExchangeRateListItem IDR = new ExchangeRateListItem(); IDR.CurrencyCode = "IDR"; IDR.ExchangeRate = exchangeRates.IDR; exchangeRateList.Add(IDR); break;
                    case "ILS": ExchangeRateListItem ILS = new ExchangeRateListItem(); ILS.CurrencyCode = "ILS"; ILS.ExchangeRate = exchangeRates.ILS; exchangeRateList.Add(ILS); break;
                    case "INR": ExchangeRateListItem INR = new ExchangeRateListItem(); INR.CurrencyCode = "INR"; INR.ExchangeRate = exchangeRates.INR; exchangeRateList.Add(INR); break;
                    case "IQD": ExchangeRateListItem IQD = new ExchangeRateListItem(); IQD.CurrencyCode = "IQD"; IQD.ExchangeRate = exchangeRates.IQD; exchangeRateList.Add(IQD); break;
                    case "IRR": ExchangeRateListItem IRR = new ExchangeRateListItem(); IRR.CurrencyCode = "IRR"; IRR.ExchangeRate = exchangeRates.IRR; exchangeRateList.Add(IRR); break;
                    case "ISK": ExchangeRateListItem ISK = new ExchangeRateListItem(); ISK.CurrencyCode = "ISK"; ISK.ExchangeRate = exchangeRates.ISK; exchangeRateList.Add(ISK); break;
                    case "JMD": ExchangeRateListItem JMD = new ExchangeRateListItem(); JMD.CurrencyCode = "JMD"; JMD.ExchangeRate = exchangeRates.JMD; exchangeRateList.Add(JMD); break;
                    case "JOD": ExchangeRateListItem JOD = new ExchangeRateListItem(); JOD.CurrencyCode = "JOD"; JOD.ExchangeRate = exchangeRates.JOD; exchangeRateList.Add(JOD); break;
                    case "JPY": ExchangeRateListItem JPY = new ExchangeRateListItem(); JPY.CurrencyCode = "JPY"; JPY.ExchangeRate = exchangeRates.JPY; exchangeRateList.Add(JPY); break;
                    case "KES": ExchangeRateListItem KES = new ExchangeRateListItem(); KES.CurrencyCode = "KES"; KES.ExchangeRate = exchangeRates.KES; exchangeRateList.Add(KES); break;
                    case "KHR": ExchangeRateListItem KHR = new ExchangeRateListItem(); KHR.CurrencyCode = "KHR"; KHR.ExchangeRate = exchangeRates.KHR; exchangeRateList.Add(KHR); break;
                    case "KRW": ExchangeRateListItem KRW = new ExchangeRateListItem(); KRW.CurrencyCode = "KRW"; KRW.ExchangeRate = exchangeRates.KRW; exchangeRateList.Add(KRW); break;
                    case "KWD": ExchangeRateListItem KWD = new ExchangeRateListItem(); KWD.CurrencyCode = "KWD"; KWD.ExchangeRate = exchangeRates.KWD; exchangeRateList.Add(KWD); break;
                    case "KZT": ExchangeRateListItem KZT = new ExchangeRateListItem(); KZT.CurrencyCode = "KZT"; KZT.ExchangeRate = exchangeRates.KZT; exchangeRateList.Add(KZT); break;
                    case "LAK": ExchangeRateListItem LAK = new ExchangeRateListItem(); LAK.CurrencyCode = "LAK"; LAK.ExchangeRate = exchangeRates.LAK; exchangeRateList.Add(LAK); break;
                    case "LBP": ExchangeRateListItem LBP = new ExchangeRateListItem(); LBP.CurrencyCode = "LBP"; LBP.ExchangeRate = exchangeRates.LBP; exchangeRateList.Add(LBP); break;
                    case "LKR": ExchangeRateListItem LKR = new ExchangeRateListItem(); LKR.CurrencyCode = "LKR"; LKR.ExchangeRate = exchangeRates.LKR; exchangeRateList.Add(LKR); break;
                    case "MAD": ExchangeRateListItem MAD = new ExchangeRateListItem(); MAD.CurrencyCode = "MAD"; MAD.ExchangeRate = exchangeRates.MAD; exchangeRateList.Add(MAD); break;
                    case "MKD": ExchangeRateListItem MKD = new ExchangeRateListItem(); MKD.CurrencyCode = "MKD"; MKD.ExchangeRate = exchangeRates.MKD; exchangeRateList.Add(MKD); break;
                    case "MMK": ExchangeRateListItem MMK = new ExchangeRateListItem(); MMK.CurrencyCode = "MMK"; MMK.ExchangeRate = exchangeRates.MMK; exchangeRateList.Add(MMK); break;
                    case "MUR": ExchangeRateListItem MUR = new ExchangeRateListItem(); MUR.CurrencyCode = "MUR"; MUR.ExchangeRate = exchangeRates.MUR; exchangeRateList.Add(MUR); break;
                    case "MXN": ExchangeRateListItem MXN = new ExchangeRateListItem(); MXN.CurrencyCode = "MXN"; MXN.ExchangeRate = exchangeRates.MXN; exchangeRateList.Add(MXN); break;
                    case "MYR": ExchangeRateListItem MYR = new ExchangeRateListItem(); MYR.CurrencyCode = "MYR"; MYR.ExchangeRate = exchangeRates.MYR; exchangeRateList.Add(MYR); break;
                    case "NAD": ExchangeRateListItem NAD = new ExchangeRateListItem(); NAD.CurrencyCode = "NAD"; NAD.ExchangeRate = exchangeRates.NAD; exchangeRateList.Add(NAD); break;
                    case "NGN": ExchangeRateListItem NGN = new ExchangeRateListItem(); NGN.CurrencyCode = "NGN"; NGN.ExchangeRate = exchangeRates.NGN; exchangeRateList.Add(NGN); break;
                    case "NOK": ExchangeRateListItem NOK = new ExchangeRateListItem(); NOK.CurrencyCode = "NOK"; NOK.ExchangeRate = exchangeRates.NOK; exchangeRateList.Add(NOK); break;
                    case "NZD": ExchangeRateListItem NZD = new ExchangeRateListItem(); NZD.CurrencyCode = "NZD"; NZD.ExchangeRate = exchangeRates.NZD; exchangeRateList.Add(NZD); break;
                    case "OMR": ExchangeRateListItem OMR = new ExchangeRateListItem(); OMR.CurrencyCode = "OMR"; OMR.ExchangeRate = exchangeRates.OMR; exchangeRateList.Add(OMR); break;
                    case "PAB": ExchangeRateListItem PAB = new ExchangeRateListItem(); PAB.CurrencyCode = "PAB"; PAB.ExchangeRate = exchangeRates.PAB; exchangeRateList.Add(PAB); break;
                    case "PEN": ExchangeRateListItem PEN = new ExchangeRateListItem(); PEN.CurrencyCode = "PEN"; PEN.ExchangeRate = exchangeRates.PEN; exchangeRateList.Add(PEN); break;
                    case "PGK": ExchangeRateListItem PGK = new ExchangeRateListItem(); PGK.CurrencyCode = "PGK"; PGK.ExchangeRate = exchangeRates.PGK; exchangeRateList.Add(PGK); break;
                    case "PHP": ExchangeRateListItem PHP = new ExchangeRateListItem(); PHP.CurrencyCode = "PHP"; PHP.ExchangeRate = exchangeRates.PHP; exchangeRateList.Add(PHP); break;
                    case "PKR": ExchangeRateListItem PKR = new ExchangeRateListItem(); PKR.CurrencyCode = "PKR"; PKR.ExchangeRate = exchangeRates.PKR; exchangeRateList.Add(PKR); break;
                    case "PLN": ExchangeRateListItem PLN = new ExchangeRateListItem(); PLN.CurrencyCode = "PLN"; PLN.ExchangeRate = exchangeRates.PLN; exchangeRateList.Add(PLN); break;
                    case "PYG": ExchangeRateListItem PYG = new ExchangeRateListItem(); PYG.CurrencyCode = "PYG"; PYG.ExchangeRate = exchangeRates.PYG; exchangeRateList.Add(PYG); break;
                    case "QAR": ExchangeRateListItem QAR = new ExchangeRateListItem(); QAR.CurrencyCode = "QAR"; QAR.ExchangeRate = exchangeRates.QAR; exchangeRateList.Add(QAR); break;
                    case "RON": ExchangeRateListItem RON = new ExchangeRateListItem(); RON.CurrencyCode = "RON"; RON.ExchangeRate = exchangeRates.RON; exchangeRateList.Add(RON); break;
                    case "RSD": ExchangeRateListItem RSD = new ExchangeRateListItem(); RSD.CurrencyCode = "RSD"; RSD.ExchangeRate = exchangeRates.RSD; exchangeRateList.Add(RSD); break;
                    case "RUB": ExchangeRateListItem RUB = new ExchangeRateListItem(); RUB.CurrencyCode = "RUB"; RUB.ExchangeRate = exchangeRates.RUB; exchangeRateList.Add(RUB); break;
                    case "SAR": ExchangeRateListItem SAR = new ExchangeRateListItem(); SAR.CurrencyCode = "SAR"; SAR.ExchangeRate = exchangeRates.SAR; exchangeRateList.Add(SAR); break;
                    case "SCR": ExchangeRateListItem SCR = new ExchangeRateListItem(); SCR.CurrencyCode = "SCR"; SCR.ExchangeRate = exchangeRates.SCR; exchangeRateList.Add(SCR); break;
                    case "SEK": ExchangeRateListItem SEK = new ExchangeRateListItem(); SEK.CurrencyCode = "SEK"; SEK.ExchangeRate = exchangeRates.SEK; exchangeRateList.Add(SEK); break;
                    case "SGD": ExchangeRateListItem SGD = new ExchangeRateListItem(); SGD.CurrencyCode = "SGD"; SGD.ExchangeRate = exchangeRates.SGD; exchangeRateList.Add(SGD); break;
                    case "THB": ExchangeRateListItem THB = new ExchangeRateListItem(); THB.CurrencyCode = "THB"; THB.ExchangeRate = exchangeRates.THB; exchangeRateList.Add(THB); break;
                    case "TJS": ExchangeRateListItem TJS = new ExchangeRateListItem(); TJS.CurrencyCode = "TJS"; TJS.ExchangeRate = exchangeRates.TJS; exchangeRateList.Add(TJS); break;
                    case "TND": ExchangeRateListItem TND = new ExchangeRateListItem(); TND.CurrencyCode = "TND"; TND.ExchangeRate = exchangeRates.TND; exchangeRateList.Add(TND); break;
                    case "TRY": ExchangeRateListItem TRY = new ExchangeRateListItem(); TRY.CurrencyCode = "TRY"; TRY.ExchangeRate = exchangeRates.TRY; exchangeRateList.Add(TRY); break;
                    case "TTD": ExchangeRateListItem TTD = new ExchangeRateListItem(); TTD.CurrencyCode = "TTD"; TTD.ExchangeRate = exchangeRates.TTD; exchangeRateList.Add(TTD); break;
                    case "TWD": ExchangeRateListItem TWD = new ExchangeRateListItem(); TWD.CurrencyCode = "TWD"; TWD.ExchangeRate = exchangeRates.TWD; exchangeRateList.Add(TWD); break;
                    case "TZS": ExchangeRateListItem TZS = new ExchangeRateListItem(); TZS.CurrencyCode = "TZS"; TZS.ExchangeRate = exchangeRates.TZS; exchangeRateList.Add(TZS); break;
                    case "UAH": ExchangeRateListItem UAH = new ExchangeRateListItem(); UAH.CurrencyCode = "UAH"; UAH.ExchangeRate = exchangeRates.UAH; exchangeRateList.Add(UAH); break;
                    case "USD": ExchangeRateListItem USD = new ExchangeRateListItem(); USD.CurrencyCode = "USD"; USD.ExchangeRate = exchangeRates.USD; exchangeRateList.Add(USD); break;
                    case "UYU": ExchangeRateListItem UYU = new ExchangeRateListItem(); UYU.CurrencyCode = "UYU"; UYU.ExchangeRate = exchangeRates.UYU; exchangeRateList.Add(UYU); break;
                    case "UZS": ExchangeRateListItem UZS = new ExchangeRateListItem(); UZS.CurrencyCode = "UZS"; UZS.ExchangeRate = exchangeRates.UZS; exchangeRateList.Add(UZS); break;
                    case "VEF": ExchangeRateListItem VEF = new ExchangeRateListItem(); VEF.CurrencyCode = "VEF"; VEF.ExchangeRate = exchangeRates.VEF; exchangeRateList.Add(VEF); break;
                    case "VND": ExchangeRateListItem VND = new ExchangeRateListItem(); VND.CurrencyCode = "VND"; VND.ExchangeRate = exchangeRates.VND; exchangeRateList.Add(VND); break;
                    case "XAF": ExchangeRateListItem XAF = new ExchangeRateListItem(); XAF.CurrencyCode = "XAF"; XAF.ExchangeRate = exchangeRates.XAF; exchangeRateList.Add(XAF); break;
                    case "XCD": ExchangeRateListItem XCD = new ExchangeRateListItem(); XCD.CurrencyCode = "XCD"; XCD.ExchangeRate = exchangeRates.XCD; exchangeRateList.Add(XCD); break;
                    case "XOF": ExchangeRateListItem XOF = new ExchangeRateListItem(); XOF.CurrencyCode = "XOF"; XOF.ExchangeRate = exchangeRates.XOF; exchangeRateList.Add(XOF); break;
                    case "XPF": ExchangeRateListItem XPF = new ExchangeRateListItem(); XPF.CurrencyCode = "XPF"; XPF.ExchangeRate = exchangeRates.XPF; exchangeRateList.Add(XPF); break;
                    case "ZAR": ExchangeRateListItem ZAR = new ExchangeRateListItem(); ZAR.CurrencyCode = "ZAR"; ZAR.ExchangeRate = exchangeRates.ZAR; exchangeRateList.Add(ZAR); break;
                    case "ZMW": ExchangeRateListItem ZMW = new ExchangeRateListItem(); ZMW.CurrencyCode = "ZMW"; ZMW.ExchangeRate = exchangeRates.ZMW; exchangeRateList.Add(ZMW); break;
                                         



                }
            }

          //  join n in exchangeRateList on m.CurrencyCode.Trim() equals n.CurrencyCode.Trim()
            //    m in m_baseRepository.GetQuery<Currency>(m => m.CurrencyCode != baseCurrency).ToList()
            var exchangeRate1 = (from m in exchangeRateList.Where(x=>x.CurrencyCode!=baseCurrency)
                                 join n in m_baseRepository.GetQuery<Currency>().ToList() on m.CurrencyCode.Trim() equals n.CurrencyCode.Trim()
                     join k in m_baseRepository.GetQuery<ExchangeRate>().ToList() on n.CurrencyCodeNum equals k.CurrencyCodeNum
                     select new ExchangeRate()
                     {
                         MeritExchangeRate = m.ExchangeRate,
                         BonusExchangeRate = m.ExchangeRate,
                         Active = k.Active,
                         CurrencyCodeNum=k.CurrencyCodeNum,
                         ExchangeRateNum=k.ExchangeRateNum,
                   }
                   ).Distinct().ToList();
            foreach (var item in exchangeRate1)
            {
                ExchangeRate s = m_baseRepository.GetQuery<ExchangeRate>(m => m.ExchangeRateNum == item.ExchangeRateNum).FirstOrDefault();
                s.MeritExchangeRate = item.MeritExchangeRate;
                s.BonusExchangeRate = item.MeritExchangeRate;
                s.Active = item.Active;
                m_baseRepository.Edit<ExchangeRate>(s);
                m_baseRepository.SaveChanges();
               // result.Add(s);
            }
          
            return 0;
          
               
                }
        public ExchangeRatePreviewData GetPreviewData()
      {
          ExchangeRatePreviewData model =new ExchangeRatePreviewData();
            var appSetting = m_tenantCacheProvider.GetApplicationSetting();
          model.ConversionPreview =Convert.ToDecimal(appSetting.CurrencyConversionPreview);
          model.DisplayPreview  =Convert.ToDecimal(appSetting.CurrencyDisplayPreview);
          return model; 
      }

       public int UpdateBaseCurrency(CultureCodeData selectedBaseCurrency)
        {
            var appsetting = m_baseRepository.GetQuery<AppSetting>(x => x.AppSettingID == "BaseCurrency").FirstOrDefault();
            appsetting.AppSettingValue = selectedBaseCurrency.CurrencyCode;
            m_baseRepository.Edit<AppSetting>(appsetting);
            m_tenantCacheProvider.RemoveCache(ApplicationCacheConstants.ApplicationSetting);
            var exchangeRate = m_baseRepository.GetQuery<ExchangeRate>(x => x.CurrencyCodeNum == selectedBaseCurrency.CurrencyCodeNum).FirstOrDefault();
            exchangeRate.MeritExchangeRate = 1;
            exchangeRate.BonusExchangeRate = 1;
            m_baseRepository.Edit<ExchangeRate>(exchangeRate);
            m_baseRepository.SaveChanges();
            return 0;
        }

        public bool ValidateCurrencyCode(string currencyCodeValue)
        {
            string isCurrencyCodeExists = m_baseRepository.GetQuery<Currency>().Where(x => x.CurrencyCode == currencyCodeValue).Select(x => x.CurrencyCode).FirstOrDefault();
            if(isCurrencyCodeExists != null){
                return true;
            }
            else{
                return false;
            }
        }

        // Author       : Shaheena Shaik
        // Creation date: 30-June-2017
        /// <summary>
        /// Getting ExchangeRate Export data from database
        /// </summary>
        /// <returns>Returning ExchangeRate Export</returns>
        public async Task<DataTable> GetExchangeRateExportData()
        {
            return await m_baseRepository.GetDataTableFromStoredProcedure("[Common].[USP_ExchangeRate_GET_ExchangeRateExport]");
        }
    }
    }

