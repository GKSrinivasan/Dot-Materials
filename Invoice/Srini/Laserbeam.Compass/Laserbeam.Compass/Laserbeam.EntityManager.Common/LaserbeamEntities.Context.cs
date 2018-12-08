﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class LaserbeamEntitiesContainer1 : DbContext
    {
        public LaserbeamEntitiesContainer1()
            : base("name=LaserbeamEntitiesContainer1")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AppEmail> AppEmails { get; set; }
        public virtual DbSet<AppExportScript> AppExportScripts { get; set; }
        public virtual DbSet<AppMenu> AppMenus { get; set; }
        public virtual DbSet<AppMessage> AppMessages { get; set; }
        public virtual DbSet<AppScript> AppScripts { get; set; }
        public virtual DbSet<AppSetting> AppSettings { get; set; }
        public virtual DbSet<AppTable> AppTables { get; set; }
        public virtual DbSet<AppUserRole> AppUserRoles { get; set; }
        public virtual DbSet<AppUserStatu> AppUserStatus { get; set; }
        public virtual DbSet<BonusType> BonusTypes { get; set; }
        public virtual DbSet<BusinessUnit> BusinessUnits { get; set; }
        public virtual DbSet<BusSetting> BusSettings { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<CompensationType> CompensationTypes { get; set; }
        public virtual DbSet<CostCenter> CostCenters { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<DateFormat> DateFormats { get; set; }
        public virtual DbSet<DecimalType> DecimalTypes { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Division> Divisions { get; set; }
        public virtual DbSet<EmailTracker> EmailTrackers { get; set; }
        public virtual DbSet<EmployeeClass> EmployeeClasses { get; set; }
        public virtual DbSet<EntityCode> EntityCodes { get; set; }
        public virtual DbSet<Ethnicity> Ethnicities { get; set; }
        public virtual DbSet<ExchangeRate> ExchangeRates { get; set; }
        public virtual DbSet<Family> Families { get; set; }
        public virtual DbSet<Grade> Grades { get; set; }
        public virtual DbSet<GroupType> GroupTypes { get; set; }
        public virtual DbSet<InitiativesLink> InitiativesLinks { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<JobStatu> JobStatus { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<MetaColumn> MetaColumns { get; set; }
        public virtual DbSet<MetaTable> MetaTables { get; set; }
        public virtual DbSet<MetaXmlTemplate> MetaXmlTemplates { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<Operator> Operators { get; set; }
        public virtual DbSet<OrgGroupCriteria> OrgGroupCriterias { get; set; }
        public virtual DbSet<PayGroup> PayGroups { get; set; }
        public virtual DbSet<Program> Programs { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<ReviewType> ReviewTypes { get; set; }
        public virtual DbSet<RoundingType> RoundingTypes { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<SubFamily> SubFamilies { get; set; }
        public virtual DbSet<SubStatu> SubStatus { get; set; }
        public virtual DbSet<UserAppUrlTracker> UserAppUrlTrackers { get; set; }
        public virtual DbSet<UserQuery> UserQueries { get; set; }
        public virtual DbSet<XmlProcess> XmlProcesses { get; set; }
        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<Budget> Budgets { get; set; }
        public virtual DbSet<DailyTask> DailyTasks { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeApprovalDetail> EmployeeApprovalDetails { get; set; }
        public virtual DbSet<EmployeeCompAdjustment> EmployeeCompAdjustments { get; set; }
        public virtual DbSet<EmployeeCompApprovalComment> EmployeeCompApprovalComments { get; set; }
        public virtual DbSet<EmployeeCompApprovalLevel> EmployeeCompApprovalLevels { get; set; }
        public virtual DbSet<EmployeeCompApprovalRequest> EmployeeCompApprovalRequests { get; set; }
        public virtual DbSet<EmployeeCompApprovalStatu> EmployeeCompApprovalStatus { get; set; }
        public virtual DbSet<EmployeeCompComment> EmployeeCompComments { get; set; }
        public virtual DbSet<EmployeeCompCommentStatu> EmployeeCompCommentStatus { get; set; }
        public virtual DbSet<EmployeeCompMarket> EmployeeCompMarkets { get; set; }
        public virtual DbSet<EmployeeCompMerit> EmployeeCompMerits { get; set; }
        public virtual DbSet<EmployeeCompNew> EmployeeCompNews { get; set; }
        public virtual DbSet<EmployeeCompPromotion> EmployeeCompPromotions { get; set; }
        public virtual DbSet<EmployeeCompRating> EmployeeCompRatings { get; set; }
        public virtual DbSet<EmployeeDetailsCorrection> EmployeeDetailsCorrections { get; set; }
        public virtual DbSet<EmployeeGroup> EmployeeGroups { get; set; }
        public virtual DbSet<EmployeeGroupInclusionExclusion> EmployeeGroupInclusionExclusions { get; set; }
        public virtual DbSet<EmployeeInclusionAndExclusion> EmployeeInclusionAndExclusions { get; set; }
        public virtual DbSet<EmployeeJob> EmployeeJobs { get; set; }
        public virtual DbSet<EmployeeMoreInfo> EmployeeMoreInfoes { get; set; }
        public virtual DbSet<EmployeeWorkFlowChanx> EmployeeWorkFlowChanges { get; set; }
        public virtual DbSet<EmployeeWorkflowCommentStatu> EmployeeWorkflowCommentStatus { get; set; }
        public virtual DbSet<FeedBack> FeedBacks { get; set; }
        public virtual DbSet<ManagerRelation> ManagerRelations { get; set; }
        public virtual DbSet<OrgGroup> OrgGroups { get; set; }
        public virtual DbSet<OrgGroupAssignedCriteria> OrgGroupAssignedCriterias { get; set; }
        public virtual DbSet<OrgGroupDetail> OrgGroupDetails { get; set; }
        public virtual DbSet<OrgGroupType> OrgGroupTypes { get; set; }
        public virtual DbSet<ProxyGroup> ProxyGroups { get; set; }
        public virtual DbSet<ProxyManager> ProxyManagers { get; set; }
        public virtual DbSet<ProxyPopulationAccess> ProxyPopulationAccesses { get; set; }
        public virtual DbSet<ProxyScript> ProxyScripts { get; set; }
        public virtual DbSet<TriggerEmail> TriggerEmails { get; set; }
        public virtual DbSet<UserMenu> UserMenus { get; set; }
        public virtual DbSet<UserMessage> UserMessages { get; set; }
        public virtual DbSet<UserPopulation> UserPopulations { get; set; }
        public virtual DbSet<PivotedEmployeeCompApprovalLevel> PivotedEmployeeCompApprovalLevels { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<SupportTask> SupportTasks { get; set; }
        public virtual DbSet<SupportTaskComment> SupportTaskComments { get; set; }
        public virtual DbSet<ChatUserStatus> ChatUserStatus1 { get; set; }
        public virtual DbSet<ChatDetail> ChatDetails { get; set; }
        public virtual DbSet<ChatSessionDetail> ChatSessionDetails { get; set; }
        public virtual DbSet<MarketPayRange> MarketPayRanges { get; set; }
        public virtual DbSet<EmployeeCompBonu> EmployeeCompBonus { get; set; }
    
        [DbFunction("LaserbeamEntitiesContainer1", "fn_GetAbbreviation")]
        public virtual IQueryable<string> fn_GetAbbreviation(string sep, string s)
        {
            var sepParameter = sep != null ?
                new ObjectParameter("sep", sep) :
                new ObjectParameter("sep", typeof(string));
    
            var sParameter = s != null ?
                new ObjectParameter("s", s) :
                new ObjectParameter("s", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<string>("[LaserbeamEntitiesContainer1].[fn_GetAbbreviation](@sep, @s)", sepParameter, sParameter);
        }
    }
}