// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name  :    RuleConfigurationRepository
// Description     : 	Repository for RuleConfiguration
// Author          :	Raja Ganapathy
// Creation Date   : 	30-Mar-2017 

using Laserbeam.BusinessObject.Common;
using Laserbeam.BusinessObject.Common.CachedModels;
using Laserbeam.BusinessObject.Common.Constants;
using Laserbeam.DataManager.Interfaces.Common;
using Laserbeam.DataManager.Interfaces.Core;
using Laserbeam.EntityManager.Common;
using Laserbeam.Libraries.Core.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Laserbeam.DataManager.Common
{
    public class RuleConfigurationRepository : IRuleConfigurationRepository
    {
        #region Fields
        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// Object of IBaseRepository
        /// </summary>
        private IBaseRepository m_baseRepository;

        private ITenantCacheProvider m_tenantCacheProvider;
        #endregion

        #region Constructors
        // Author         :   Raja Ganapathy
        // Creation Date  :   30-Mar-2017 
        /// <summary>
        /// Initialize the base repository
        /// </summary>
        /// <param name="workForceProcessManager">Object of IBaseRepository</param>   
        public RuleConfigurationRepository(IBaseRepository baseRepository, ITenantCacheProvider tenantCacheProvider)
        {
            m_baseRepository = baseRepository;
            m_tenantCacheProvider = tenantCacheProvider;
        }
        #endregion

        #region Public Methods       

        
        public IQueryable<DateFormat> GetDateFormats()
        {
            return m_baseRepository.GetQuery<DateFormat>(x => x.Active == true);
           
        }

        public int UpdateExchangeRate(Countries exchangeRates, string baseCurrency)
        {
            var a = m_baseRepository.GetQuery<Currency>().ToList();
            List<ExchangeRateListItem> exchangeRateList = new List<ExchangeRateListItem>();
            List<ExchangeRate> result = new List<ExchangeRate>();
            var currency = m_baseRepository.GetQuery<Currency>();
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
            var exchangeRate1 = (from m in exchangeRateList.Where(x => x.CurrencyCode != baseCurrency)
                                 join n in m_baseRepository.GetQuery<Currency>().ToList() on m.CurrencyCode.Trim() equals n.CurrencyCode.Trim()
                                 join k in m_baseRepository.GetQuery<ExchangeRate>().ToList() on n.CurrencyCodeNum equals k.CurrencyCodeNum
                                 select new ExchangeRate()
                                 {
                                     MeritExchangeRate = m.ExchangeRate,
                                     BonusExchangeRate = m.ExchangeRate,
                                     Active = k.Active,
                                     CurrencyCodeNum = k.CurrencyCodeNum,
                                     ExchangeRateNum = k.ExchangeRateNum,
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

        public async Task PutBusSetting(BusSettingModel BusSettingDetails, int UserNum)
        {
            List<BusSetting> busSetting = m_baseRepository.GetQuery<BusSetting>().ToList();
            List<AppSetting> appSetting = m_baseRepository.GetQuery<AppSetting>().ToList();
           // BusSettingEdit(busSetting, "GlobalPortion", BusSettingDetails.BonusGlobalPortion, UserNum);
           // BusSettingEdit(busSetting, "IndividualPortion", BusSettingDetails.BonusIndividualPortion, UserNum);
            BusSettingEdit(busSetting, "EitherMeritOrLumpSum", BusSettingDetails.EitherMeritOrLumpSum, UserNum);
            BusSettingEdit(busSetting, "EnableSSO", BusSettingDetails.EnableSSO, UserNum);
            BusSettingEdit(busSetting, "IDPEndPoint", BusSettingDetails.IDPEndPoint, UserNum);
            BusSettingEdit(busSetting, "Merit", BusSettingDetails.Merit, UserNum);
            BusSettingEdit(busSetting, "Bonus", BusSettingDetails.Bonus, UserNum);
            BusSettingEdit(busSetting, "Adjustment", BusSettingDetails.Adjustment, UserNum);
            BusSettingEdit(busSetting, "Lumpsum", BusSettingDetails.Lumpsum, UserNum);            
            BusSettingEdit(busSetting, "Prorate", BusSettingDetails.Prorate, UserNum);
            BusSettingEdit(busSetting, "RatingDisplay", BusSettingDetails.RatingDisplay, UserNum);
            BusSettingEdit(busSetting, "RatingDropdown", BusSettingDetails.RatingDropdown, UserNum);            
            BusSettingEdit(busSetting, "MultiCurrency", BusSettingDetails.MultiCurrency, UserNum);
            BusSettingEdit(busSetting, "Promotion", BusSettingDetails.Promotion, UserNum);
            BusSettingEdit(busSetting, "MeritCalculation", BusSettingDetails.MeritCalculation, UserNum);
            BusSettingEdit(busSetting, "CurrencyCode", BusSettingDetails.CurrencyCode, UserNum);            
            BusSettingEdit(busSetting, "ApplyBudgetCalculations", BusSettingDetails.ApplyBudgetCalculations, UserNum);            
            BusSettingEdit(busSetting, "ProrateIncreaseStartDate", (Convert.ToDateTime(BusSettingDetails.ProrateIncreaseStartDate)).ToString("yyyy-MM-dd"), UserNum);
            BusSettingEdit(busSetting, "ProrateIncreaseEndDate", (Convert.ToDateTime(BusSettingDetails.ProrateIncreaseEndDate)).ToString("yyyy-MM-dd"), UserNum);
            BusSettingEdit(busSetting, "ProrationType", BusSettingDetails.ProrationType, UserNum);
            BusSettingEdit(busSetting, "ProrationLength", BusSettingDetails.ProrationLength, UserNum);
            BusSettingEdit(busSetting, "ProrationLengthtoInclude", BusSettingDetails.ProrationLengthtoInclude, UserNum);            
            //BusSettingEdit(busSetting, "LumpsumType", BusSettingDetails.LumpsumType, UserNum);
            //BusSettingEdit(busSetting, "RangeMaxPct", BusSettingDetails.RangeMaxPct, UserNum);
            //BusSettingEdit(busSetting, "RangeMaxAmt", BusSettingDetails.RangeMaxAmt, UserNum);
            BusSettingEdit(busSetting, "MeritOverrideHardStop", BusSettingDetails.MeritOverrideHardStop, UserNum);
            BusSettingEdit(busSetting, "MeritOverrideSoftStop", BusSettingDetails.MeritOverrideSoftStop, UserNum);
            BusSettingEdit(busSetting, "MeritIncreaseWithinGuideline", BusSettingDetails.MeritIncreaseWithinGuideline, UserNum);
            BusSettingEdit(busSetting, "MandatoryJustification", BusSettingDetails.MandatoryJustification, UserNum);
            BusSettingEdit(busSetting, "MeritOverrideNoJustification", BusSettingDetails.MeritOverrideNoJustification, UserNum);            
            BusSettingEdit(busSetting, "RoundingMeritPct", BusSettingDetails.RoundingMeritPct, UserNum);
            BusSettingEdit(busSetting, "RoundingMeritHourly", BusSettingDetails.RoundingMeritHourly, UserNum);
            BusSettingEdit(busSetting, "RoundingMeritAnnual", BusSettingDetails.RoundingMeritAnnual, UserNum);
            BusSettingEdit(busSetting, "DecimalMeritPct", BusSettingDetails.DecimalMeritPct, UserNum);
            BusSettingEdit(busSetting, "DecimalMeritHourly", BusSettingDetails.DecimalMeritHourly, UserNum);
            BusSettingEdit(busSetting, "DecimalMeritAnnual", BusSettingDetails.DecimalMeritAnnual, UserNum);
            BusSettingEdit(busSetting, "RoundingLumpSumPct", BusSettingDetails.RoundingLumpSumPct, UserNum);
            BusSettingEdit(busSetting, "RoundingLumpSumHourly", BusSettingDetails.RoundingLumpSumHourly, UserNum);
            BusSettingEdit(busSetting, "RoundingLumpSumAnnual", BusSettingDetails.RoundingLumpSumAnnual, UserNum);
            BusSettingEdit(busSetting, "DecimalLumpSumPct", BusSettingDetails.DecimalLumpSumPct, UserNum);
            BusSettingEdit(busSetting, "DecimalLumpSumHourly", BusSettingDetails.DecimalLumpSumHourly, UserNum);
            BusSettingEdit(busSetting, "DecimalLumpSumAnnual", BusSettingDetails.DecimalLumpSumAnnual, UserNum);
            BusSettingEdit(busSetting, "RoundingAdjustmentPct", BusSettingDetails.RoundingAdjustmentPct, UserNum);
            BusSettingEdit(busSetting, "RoundingAdjustmentHourly", BusSettingDetails.RoundingAdjustmentHourly, UserNum);
            BusSettingEdit(busSetting, "RoundingAdjustmentAnnual", BusSettingDetails.RoundingAdjustmentAnnual, UserNum);
            BusSettingEdit(busSetting, "DecimalAdjustmentPct", BusSettingDetails.DecimalAdjustmentPct, UserNum);
            BusSettingEdit(busSetting, "DecimalAdjustmentHourly", BusSettingDetails.DecimalAdjustmentHourly, UserNum);
            BusSettingEdit(busSetting, "DecimalAdjustmentAnnual", BusSettingDetails.DecimalAdjustmentAnnual, UserNum);
            BusSettingEdit(busSetting, "RoundingCompaRatioPct", BusSettingDetails.RoundingCompaRatioPct, UserNum);
            BusSettingEdit(busSetting, "RoundingCompaRatioHourly", BusSettingDetails.RoundingCompaRatioHourly, UserNum);
            BusSettingEdit(busSetting, "RoundingCompaRatioAnnual", BusSettingDetails.RoundingCompaRatioAnnual, UserNum);
            BusSettingEdit(busSetting, "DecimalCompaRatioPct", BusSettingDetails.DecimalCompaRatioPct, UserNum);
            BusSettingEdit(busSetting, "DecimalCompaRatioHourly", BusSettingDetails.DecimalCompaRatioHourly, UserNum);
            BusSettingEdit(busSetting, "DecimalCompaRatioAnnual", BusSettingDetails.DecimalCompaRatioAnnual, UserNum);            
            BusSettingEdit(busSetting, "RoundingPromotionPct", BusSettingDetails.RoundingPromotionPct, UserNum);
            BusSettingEdit(busSetting, "RoundingPromotionHourly", BusSettingDetails.RoundingPromotionHourly, UserNum);
            BusSettingEdit(busSetting, "RoundingPromotionAnnual", BusSettingDetails.RoundingPromotionAnnual, UserNum);
            BusSettingEdit(busSetting, "DecimalPromotionPct", BusSettingDetails.DecimalPromotionPct, UserNum);
            BusSettingEdit(busSetting, "DecimalPromotionHourly", BusSettingDetails.DecimalPromotionHourly, UserNum);
            BusSettingEdit(busSetting, "DecimalPromotionAnnual", BusSettingDetails.DecimalPromotionAnnual, UserNum);
            BusSettingEdit(busSetting, "TCC", BusSettingDetails.TCC, UserNum);            
            BusSettingEdit(busSetting, "EmployeeNameFormat", BusSettingDetails.EmployeeNameFormat, UserNum);            
            BusSettingEdit(busSetting, "UserNameFormat", BusSettingDetails.UserNameFormat, UserNum);            
            BusSettingEdit(busSetting, "SortOrderEmployeeNameFormat", BusSettingDetails.SortOrderEmployeeNameFormat, UserNum);
            BusSettingEdit(busSetting, "DateFormat", BusSettingDetails.DateFormat, UserNum);
            BusSettingEdit(busSetting, "CurrencyFormat", BusSettingDetails.CurrencyFormat, UserNum);            
            BusSettingEdit(busSetting, "RoundNewSalaryPct", BusSettingDetails.RoundNewSalaryPct, UserNum);
            BusSettingEdit(busSetting, "RoundNewSalaryHourly", BusSettingDetails.RoundNewSalaryHourly, UserNum);
            BusSettingEdit(busSetting, "RoundNewSalaryAnnual", BusSettingDetails.RoundNewSalaryAnnual, UserNum);
            BusSettingEdit(busSetting, "DecimalNewSalaryPct", BusSettingDetails.DecimalNewSalaryPct, UserNum);
            BusSettingEdit(busSetting, "DecimalNewSalaryHourly", BusSettingDetails.DecimalNewSalaryHourly, UserNum);
            BusSettingEdit(busSetting, "DecimalNewSalaryAnnual", BusSettingDetails.DecimalNewSalaryAnnual, UserNum);
            //Current Salary
            BusSettingEdit(busSetting, "RoundCurrentSalaryPct", BusSettingDetails.RoundCurrentSalaryPct, UserNum);
            BusSettingEdit(busSetting, "RoundCurrentSalaryHourly", BusSettingDetails.RoundCurrentSalaryHourly, UserNum);
            BusSettingEdit(busSetting, "RoundCurrentSalaryAnnual", BusSettingDetails.RoundCurrentSalaryAnnual, UserNum);
            BusSettingEdit(busSetting, "DecimalCurrentSalaryPct", BusSettingDetails.DecimalCurrentSalaryPct, UserNum);
            BusSettingEdit(busSetting, "DecimalCurrentSalaryHourly", BusSettingDetails.DecimalCurrentSalaryHourly, UserNum);
            BusSettingEdit(busSetting, "DecimalCurrentSalaryAnnual", BusSettingDetails.DecimalCurrentSalaryAnnual, UserNum);

            ////////////////
            BusSettingEdit(busSetting, "MeritValuesReCalculate", BusSettingDetails.MeritReCalculation, UserNum);
            BusSettingEdit(busSetting, "MeritValuesReCalculate", BusSettingDetails.LumpsumType=="AutoCalc"?"YES":"NO", UserNum);
            BusSettingEdit(busSetting, "WorkFlow", BusSettingDetails.WorkFlow, UserNum);
            BusSettingEdit(busSetting, "AutoCalculateLumpSum", BusSettingDetails.AutoCalculateLumpSum, UserNum);
            BusSettingEdit(busSetting, "LumpSumRuleTurnOff", BusSettingDetails.LumpSumRuleTurnOff, UserNum);
            BusSettingEdit(busSetting, "LumpsumType", BusSettingDetails.LumpsumType, UserNum);
            BusSettingEdit(busSetting, "ComparativeRatio", BusSettingDetails.ComparativeRatio, UserNum);
            BusSettingEdit(busSetting, "RangeMaxPct", (BusSettingDetails.RangeMaxPct!=null) ? BusSettingDetails.RangeMaxPct : "", UserNum);
            BusSettingEdit(busSetting, "RangeMaxAmt", (BusSettingDetails.RangeMaxAmt!=null) ? BusSettingDetails.RangeMaxAmt : "", UserNum);
            //BusSettingEdit(busSetting, "LumpsumAutoCalcWithoutOverride", BusSettingDetails.LumpSumNoAutoCalc, UserNum);
            AppSettingEdit(appSetting, "CurrentYear", (BusSettingDetails.CurrentYear != null) ? BusSettingDetails.CurrentYear : "", UserNum);
            AppSettingEdit(appSetting, "MeritCycleYear", (BusSettingDetails.CurrentYear != null) ? BusSettingDetails.CurrentYear : "", UserNum);            
            AppSettingEdit(appSetting, "PasswordLength", (BusSettingDetails.PasswordLength != null) ? BusSettingDetails.PasswordLength : "", UserNum);
            AppSettingEdit(appSetting, "AdminEmailID", (BusSettingDetails.EmailAddress != null) ? BusSettingDetails.EmailAddress : "", UserNum);
            // AppSettingEdit(appSetting, "AdminPassword", (BusSettingDetails.EmailPassword != null) ? BusSettingDetails.EmailPassword : "", UserNum);
            if (BusSettingDetails.ApplyBudgetCalculations.ToLower() == "yes")
            {
                BusSettingEdit(busSetting, "BudgetProrate", BusSettingDetails.ApplyBudgetCalculations, UserNum);
                BusSettingEdit(busSetting, "BudgetProrateIncreaseStartDate",(Convert.ToDateTime(BusSettingDetails.ProrateIncreaseStartDate)).ToString("yyyy-MM-dd"), UserNum);
                BusSettingEdit(busSetting, "BudgetProrateIncreaseEndDate", (Convert.ToDateTime(BusSettingDetails.ProrateIncreaseEndDate)).ToString("yyyy-MM-dd"), UserNum);
                BusSettingEdit(busSetting, "BudgetProrationType", BusSettingDetails.ProrationType, UserNum);
                BusSettingEdit(busSetting, "BudgetProrationDuration", BusSettingDetails.ProrationLength, UserNum);
                BusSettingEdit(busSetting, "BudgetProrationDatesPerMonth", BusSettingDetails.ProrationLengthtoInclude, UserNum);
                await m_baseRepository.SaveChangesAsync();
                var count = await m_baseRepository.ExecuteStoredProcedure("Talent.USP_BP_PUT_ApplyBudgetProration");
            }
            else
            {
                await m_baseRepository.SaveChangesAsync();
            }
            m_tenantCacheProvider.RemoveCache(ApplicationCacheConstants.BussinessSetting);
            m_tenantCacheProvider.RemoveCache(ApplicationCacheConstants.ApplicationSetting);
        }

        public async Task RunBuildManagerTree()
        {
            SqlParameter[] parameters = { };
            await m_baseRepository.ExecuteStoredProcedure("[Talent].[USP_SLP_PUT_BuildManagerTree]", parameters);
        }


    public BusSettingModel GetBusSetting()
        {
            // List<BusSetting> busSetting = m_baseRepository.GetQuery<BusSetting>().ToList();
            BusinessSettingModel busSetting = m_tenantCacheProvider.GetBusinessSetting();
            //  var appSetting = m_baseRepository.GetQuery<AppSetting>().ToList();
            var appSetting = m_tenantCacheProvider.GetApplicationSetting();
            BusSettingModel busSettingModel = new BusSettingModel();
            busSettingModel.Merit = busSetting.FeatureConfiguration.Merit==true ? "YES" : "NO";

            busSettingModel.Adjustment = busSetting.FeatureConfiguration.Adjustment == true ? "YES" : "NO";
            busSettingModel.Lumpsum = busSetting.FeatureConfiguration.Lumpsum == true ? "YES" : "NO";
            busSettingModel.TCC = busSetting.FeatureConfiguration.TCC == true ? "YES" : "NO";
            busSettingModel.Prorate = busSetting.ProrationRule.Prorate == true ? "YES" : "NO";
            busSettingModel.RatingDisplay = busSetting.FeatureConfiguration.RatingDisplay == true ? "YES" : "NO";
            busSettingModel.RatingDropdown = busSetting.FeatureConfiguration.RatingDropdown == true ? "YES" : "NO";
            busSettingModel.MultiCurrency = busSetting.FeatureConfiguration.MultiCurrency == true ? "YES" : "NO";
            busSettingModel.Promotion = busSetting.FeatureConfiguration.Promotion == true ? "YES" : "NO";
            busSettingModel.Bonus = busSetting.FeatureConfiguration.Bonus == true ? "YES" : "NO";
            busSettingModel.CurrencyCode = busSetting.FeatureConfiguration.CurrencyCode == true ? "YES" : "NO";
            busSettingModel.MeritCalculation = busSetting.FeatureConfiguration.MeritCalculation == true ? "YES" : "NO";
            busSettingModel.ApplyBudgetCalculations = busSetting.ProrationRule.ApplyBudgetCalculations == true ? "YES" : "NO";
            busSettingModel.ProrateIncreaseStartDate = busSetting.ProrationRule.ProrateIncreaseStartDate.ToString();
            busSettingModel.ProrateIncreaseEndDate = busSetting.ProrationRule.ProrateIncreaseEndDate.ToString();
            busSettingModel.ProrationType = busSetting.ProrationRule.ProrationType.ToString();
            busSettingModel.ProrationLength = busSetting.ProrationRule.ProrationLength.ToString();
            busSettingModel.ProrationLengthtoInclude = busSetting.ProrationRule.ProrationLengthtoInclude.ToString();
            busSettingModel.LumpsumType = busSetting.LumpsumRule.LumpsumType.ToString();
            busSettingModel.RangeMaxPct = busSetting.LumpsumRule.RangeMaxPct.ToString();
            busSettingModel.RangeMaxAmt = busSetting.LumpsumRule.RangeMaxAmt.ToString();
            busSettingModel.MeritOverrideHardStop = busSetting.MeritOverrideRule.MeritOverrideHardStop == true ? "YES" : "NO";
            busSettingModel.MeritOverrideSoftStop = busSetting.MeritOverrideRule.MeritOverrideSoftStop == true ? "YES" : "NO";
            busSettingModel.MeritIncreaseWithinGuideline = busSetting.MeritOverrideRule.MeritIncreaseWithinGuideline == true ? "YES" : "NO";
            busSettingModel.MandatoryJustification = busSetting.MeritOverrideRule.MandatoryJustification == true ? "YES" : "NO";
            busSettingModel.MeritOverrideNoJustification = busSetting.MeritOverrideRule.MeritOverrideNoJustification == true ? "YES" : "NO";
            busSettingModel.RoundingMeritPct = busSetting.GeneralConfiguration.RoundingMeritPct.ToString();
            busSettingModel.RoundingMeritHourly = busSetting.GeneralConfiguration.RoundingMeritHourly.ToString();
            busSettingModel.RoundingMeritAnnual = busSetting.GeneralConfiguration.RoundingMeritAnnual.ToString();
            busSettingModel.DecimalMeritPct = busSetting.GeneralConfiguration.DecimalMeritPct.ToString();
            busSettingModel.DecimalMeritHourly = busSetting.GeneralConfiguration.DecimalMeritHourly.ToString();
            busSettingModel.DecimalMeritAnnual = busSetting.GeneralConfiguration.DecimalMeritAnnual.ToString();
            busSettingModel.RoundingLumpSumPct = busSetting.GeneralConfiguration.RoundingLumpSumPct.ToString();
            busSettingModel.RoundingLumpSumHourly = busSetting.GeneralConfiguration.RoundingLumpSumHourly.ToString();
            busSettingModel.RoundingLumpSumAnnual = busSetting.GeneralConfiguration.RoundingLumpSumAnnual.ToString();
            busSettingModel.DecimalLumpSumPct = busSetting.GeneralConfiguration.DecimalLumpSumPct.ToString();
            busSettingModel.DecimalLumpSumHourly = busSetting.GeneralConfiguration.DecimalLumpSumHourly.ToString();
            busSettingModel.DecimalLumpSumAnnual = busSetting.GeneralConfiguration.DecimalLumpSumAnnual.ToString();
            busSettingModel.RoundingAdjustmentPct = busSetting.GeneralConfiguration.RoundingAdjustmentPct.ToString();
            busSettingModel.RoundingAdjustmentHourly = busSetting.GeneralConfiguration.RoundingAdjustmentHourly.ToString();
            busSettingModel.RoundingAdjustmentAnnual = busSetting.GeneralConfiguration.RoundingAdjustmentAnnual.ToString();
            busSettingModel.DecimalAdjustmentPct = busSetting.GeneralConfiguration.DecimalAdjustmentPct.ToString();
            busSettingModel.DecimalAdjustmentHourly = busSetting.GeneralConfiguration.DecimalAdjustmentHourly.ToString();
            busSettingModel.DecimalAdjustmentAnnual = busSetting.GeneralConfiguration.DecimalAdjustmentAnnual.ToString();
            busSettingModel.RoundingCompaRatioPct = busSetting.GeneralConfiguration.RoundingCompaRatioPct.ToString();
            busSettingModel.RoundingCompaRatioHourly = busSetting.GeneralConfiguration.RoundingCompaRatioHourly.ToString();
            busSettingModel.RoundingCompaRatioAnnual = busSetting.GeneralConfiguration.RoundingCompaRatioAnnual.ToString();
            busSettingModel.DecimalCompaRatioPct = busSetting.GeneralConfiguration.DecimalCompaRatioPct.ToString();
            busSettingModel.DecimalCompaRatioHourly = busSetting.GeneralConfiguration.DecimalCompaRatioHourly.ToString();
            busSettingModel.DecimalCompaRatioAnnual = busSetting.GeneralConfiguration.DecimalCompaRatioAnnual.ToString();
            busSettingModel.RoundingPromotionPct = busSetting.GeneralConfiguration.RoundingPromotionPct.ToString();
            busSettingModel.RoundingPromotionHourly = busSetting.GeneralConfiguration.RoundingPromotionHourly.ToString();
            busSettingModel.RoundingPromotionAnnual = busSetting.GeneralConfiguration.RoundingPromotionAnnual.ToString();
            busSettingModel.DecimalPromotionPct = busSetting.GeneralConfiguration.DecimalPromotionPct.ToString();
            busSettingModel.DecimalPromotionHourly = busSetting.GeneralConfiguration.DecimalPromotionHourly.ToString();
            busSettingModel.DecimalPromotionAnnual = busSetting.GeneralConfiguration.DecimalPromotionAnnual.ToString();
            busSettingModel.EmployeeNameFormat = busSetting.NamingFormatConfiguration.EmployeeNameFormat.ToString();
            busSettingModel.UserNameFormat = busSetting.NamingFormatConfiguration.UserNameFormat.ToString();
            busSettingModel.SortOrderEmployeeNameFormat = busSetting.NamingFormatConfiguration.SortOrderEmployeeNameFormat.ToString();
            busSettingModel.DateFormat = busSetting.DateConfiguration.DateFormat.ToString();
            busSettingModel.CurrencyFormat = busSetting.CurrencyConfiguration.CurrencyFormat.ToString();
            busSettingModel.RoundNewSalaryPct = busSetting.GeneralConfiguration.RoundNewSalaryPct.ToString();
            busSettingModel.RoundNewSalaryHourly = busSetting.GeneralConfiguration.RoundNewSalaryHourly.ToString();
            busSettingModel.RoundNewSalaryAnnual = busSetting.GeneralConfiguration.RoundNewSalaryAnnual.ToString();
            busSettingModel.DecimalNewSalaryPct = busSetting.GeneralConfiguration.DecimalNewSalaryPct.ToString();
            busSettingModel.DecimalNewSalaryHourly = busSetting.GeneralConfiguration.DecimalNewSalaryHourly.ToString();
            busSettingModel.DecimalNewSalaryAnnual = busSetting.GeneralConfiguration.DecimalNewSalaryAnnual.ToString();
            busSettingModel.MeritReCalculation = busSetting.LumpsumRule.MeritValuesReCalculate == true ? "YES" : "NO";
            busSettingModel.WorkFlow = busSetting.FeatureConfiguration.WorkFlow == true ? "YES" : "NO";
            busSettingModel.EitherMeritOrLumpSum = busSetting.FeatureConfiguration.EitherMeritOrLumpSum == true ? "YES" : "NO";
            busSettingModel.AutoCalculateLumpSum = busSetting.LumpsumRule.AutoCalculateLumpSum == true ? "YES" : "NO";
            busSettingModel.LumpSumRuleTurnOff = busSetting.LumpsumRule.LumpSumRuleTurnOff == true ? "YES" : "NO";
            busSettingModel.RangeMaxPct = busSetting.LumpsumRule.RangeMaxPct.ToString();
            busSettingModel.RangeMaxAmt = busSetting.LumpsumRule.RangeMaxAmt.ToString();
            busSettingModel.ComparativeRatio = busSetting.FeatureConfiguration.ComparativeRatio == true ? "YES" : "NO";
            busSettingModel.CurrentYear = appSetting.CurrentYear.ToString();
            busSettingModel.oldYear = appSetting.CurrentYear.ToString();
            busSettingModel.PasswordLength = appSetting.PasswordLength.ToString();
            busSettingModel.EmailAddress = appSetting.AdminEmailID;
            busSettingModel.EmailPassword = appSetting.AdminPassword;
            busSettingModel.RoundCurrentSalaryPct = busSetting.GeneralConfiguration.RoundCurrentSalaryPct.ToString();
            busSettingModel.RoundCurrentSalaryHourly = busSetting.GeneralConfiguration.RoundCurrentSalaryHourly.ToString();
            busSettingModel.RoundCurrentSalaryAnnual = busSetting.GeneralConfiguration.RoundCurrentSalaryAnnual.ToString();
            busSettingModel.DecimalCurrentSalaryPct = busSetting.GeneralConfiguration.DecimalCurrentSalaryPct.ToString();
            busSettingModel.DecimalCurrentSalaryHourly = busSetting.GeneralConfiguration.DecimalCurrentSalaryHourly.ToString();
            busSettingModel.DecimalCurrentSalaryAnnual = busSetting.GeneralConfiguration.DecimalCurrentSalaryAnnual.ToString();
            busSettingModel.EnableSSO = busSetting.SSOConfiguration.EnableSSO == true ? "YES" : "NO";
            busSettingModel.IDPEndPoint = busSetting.SSOConfiguration.IDPEndPoint;

            return busSettingModel;

        }

        
        public IQueryable<RoundingType> GetRoundingType()
        {
            return m_baseRepository.GetQuery<RoundingType>();
        }

        
        public IQueryable<DecimalType> GetDecimalType()
        {
            return m_baseRepository.GetQuery<DecimalType>();
        }

      
        private void BusSettingEdit(List<BusSetting> busSetting, string key, string value, int UserNum)
        {
            var busSettingData = busSetting.Where(x => x.KeyValue.Trim() == key.Trim()).FirstOrDefault();
            if (busSettingData != null)
            {
                if (busSettingData.KeyDataValue != value)
                {
                    busSettingData.KeyDataValue = value;
                    busSettingData.UpdatedBy = UserNum;
                    busSettingData.UpdatedDate = DateTime.Now;
                    busSettingData.IsChanged = true;
                    m_baseRepository.Edit<BusSetting>(busSettingData);
                    m_baseRepository.SaveChanges();
                  
                }
            }
            //else
            //{
            //    int a = 0;
            //}
            
        }

        private void AppSettingEdit(List<AppSetting> appSetting, string key, string value, int UserNum)
        {
            var appSettingData = appSetting.Where(x => x.AppSettingID.Trim() == key.Trim()).FirstOrDefault();
            if (appSettingData != null)
            {               
                    appSettingData.AppSettingValue = value;
                    appSettingData.UpdatedBy = UserNum;
                    appSettingData.UpdatedDate = DateTime.Now;
                    m_baseRepository.Edit<AppSetting>(appSettingData);
                    m_baseRepository.SaveChanges();
               

            }
        }

        
        private string BusSettingValue(List<BusSetting> busSetting, string key)
        {
            var busSettingData = busSetting.Where(x => x.KeyValue.Trim() == key.Trim()).FirstOrDefault();
            return busSettingData != null ? Convert.ToString(busSettingData.KeyDataValue) : null;
        }

        public bool CheckMultiCurrencyChanged(string key, string value)
        {
            bool result = false;
            var busSettingData = m_tenantCacheProvider.GetBusinessSetting().FeatureConfiguration.MultiCurrency == true ? "YES" : "NO";
            if (busSettingData != null)
            {
                if (busSettingData != value)
                {
                    result = true;
                }
            }
            else
            {
                result = false;
            }
            return result;
        }

        public async Task<bool> PutApplyRules()
        {
            SqlParameter[] parameters = { };
            await m_baseRepository.ExecuteStoredProcedure("[Common].[USP_RC_PUT_ApplyRuleConfiguration]", parameters);
            return true;
        }
        public decimal GetBudgetPct()
        {
            //return Convert.ToDecimal(m_baseRepository.GetQuery<BusSetting>(x => x.KeyId == "BudgetConfiguration" && x.KeyValue == "BudgetPercent").Select(x => x.KeyDataValue).FirstOrDefault());
            return m_tenantCacheProvider.GetBusinessSetting().BudgetConfiguration.BudgetPercent ?? 0;
        }
        public bool PutWizardDetails(int userNum, byte stepInfo, bool isWizard)
        {
         

            var appUser = m_baseRepository.GetQuery<AppUser>(x => x.UserNum == userNum).FirstOrDefault();
            appUser.WizardStepsCompleted = stepInfo;
            appUser.IsWizardExpired = isWizard;
            m_baseRepository.Edit<AppUser>(appUser);
            m_baseRepository.SaveChanges();
            return true;
        }
        public byte GetWizardDetails(int userNum)
        {
            return Convert.ToByte(m_baseRepository.GetQuery<AppUser>(x => x.UserNum == userNum).Select(x => x.WizardStepsCompleted).FirstOrDefault());
        }

        public async Task<bool> clearApprovalDetails()
        {
            SqlParameter[] parameter = { };
            bool status = await m_baseRepository.ExecuteStoredProcedure("[Common].[USP_RC_ClearApprovalDetails]", parameter) > 0;
            return status;
        }

        //public bool ClearAllData()
        //{
        //    bool success;
        //    try
        //    {
        //        m_baseRepository.ExecuteStoredProcedure("[Talent].[USP_SLP_ClearAllDataScript]");
        //        success = true;
        //    }
        //    catch (Exception e)
        //    {
        //        success = false;
        //    }

        //    return success;
        //}

        #endregion
    }
}
