//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Laserbeam.EntityManager.Common
{
    using System;
    using System.Collections.Generic;
    
    public partial class ExchangeRate
    {
        public int ExchangeRateNum { get; set; }
        public int CurrencyCodeNum { get; set; }
        public Nullable<decimal> MeritExchangeRate { get; set; }
        public Nullable<decimal> BonusExchangeRate { get; set; }
        public bool Active { get; set; }
    
        public virtual Currency Currency { get; set; }
        public virtual Currency Currency1 { get; set; }
    }
}