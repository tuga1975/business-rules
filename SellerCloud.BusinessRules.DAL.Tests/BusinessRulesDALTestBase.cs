using NUnit.Framework;
using SellerCloud.BusinessRules.DAL.Services;
using System.Collections.Concurrent;

namespace SellerCloud.BusinessRules.DAL.Tests
{
    public abstract class BusinessRulesDALTestBase
    {
        private readonly ConcurrentDictionary<string, IBusinessRulesEngineContext> _dbContextDictionary;
        private readonly ConcurrentDictionary<string, IBusinessRulesEngineUnitOfWork> _unitOfWorkDictionary;

        protected readonly int ClientId = 0;

        protected IBusinessRulesEngineContext DbContext =>
            this._dbContextDictionary.GetOrAdd(TestContext.CurrentContext.Test.ID, (key) => this.CreateDbContext());
        protected IBusinessRulesEngineContext CreateDbContext() => new BusinessRulesEngineEntities();

        protected IBusinessRulesEngineUnitOfWork UnitOfWork =>
            this._unitOfWorkDictionary.GetOrAdd(TestContext.CurrentContext.Test.ID, (key) => this.CreateUnitOfWork());
        protected IBusinessRulesEngineUnitOfWork CreateUnitOfWork(IBusinessRulesEngineContext dbContext = null) => 
            new BusinessRulesEngineUnitOfWork(dbContext ?? this.DbContext);

        protected IRuleArgumentService RuleArgumentService => new RuleArgumentService(this.UnitOfWork);
        protected IRuleService RuleService => new RuleService(this.UnitOfWork, this.RuleArgumentService);
        protected IRuleModuleService RuleModuleService => new RuleModuleService(this.UnitOfWork, this.RuleService);

        public BusinessRulesDALTestBase()
        {
            this._unitOfWorkDictionary = new ConcurrentDictionary<string, IBusinessRulesEngineUnitOfWork>();
            this._dbContextDictionary = new ConcurrentDictionary<string, IBusinessRulesEngineContext>();
        }

        [SetUp]
        public void Init()
        {
            this.DbContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
        }

        [TearDown]
        public void Cleanup()
        {
            var transaction = this.DbContext.Database.CurrentTransaction;
            transaction?.Rollback();
            transaction?.Dispose();
            DbContext.Dispose();
        }
    }
}
