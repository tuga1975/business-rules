using SellerCloud.BusinessRules.DAL.Repositories;
using SellerCloud.BusinessRules.DAL.Services;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace SellerCloud.BusinessRules.DAL
{
    public class BusinessRulesEngineUnitOfWork : IBusinessRulesEngineUnitOfWork
    {
        private IBusinessRulesEngineContext dbContext;

        public IRuleModuleRepository RuleModuleRepository => new RuleModuleRepository(this.dbContext);
        public IRuleRepository RuleRepository => new RuleRepository(this.dbContext);
        public IRuleArgumentRepository RuleArgumentRepository => new RuleArgumentRepository(this.dbContext);
        
        public BusinessRulesEngineUnitOfWork(IBusinessRulesEngineContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private string FormatEntityValidationErrors(DbEntityValidationException exception)
        {
            var errors = exception.EntityValidationErrors
                    .Where(eve => !eve.IsValid)
                    .SelectMany(eve => eve.ValidationErrors.Select(v => $"{v.PropertyName}: {v.ErrorMessage}"));

            return string.Join($"; {Environment.NewLine}", errors);
        }

        public int ExecuteSqlCommand(string sqlCommand) =>
            this.dbContext.Database.ExecuteSqlCommand(sqlCommand);

        public int Commit()
        {
            try
            {
                return this.dbContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                throw new DbEntityValidationException(FormatEntityValidationErrors(ex));
            }
        }

        public async Task<int> CommitAsync()
        {
            try
            {
                return await this.dbContext.SaveChangesAsync();
            }
            catch (DbEntityValidationException ex)
            {
                throw new DbEntityValidationException(FormatEntityValidationErrors(ex));
            }
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.dbContext?.Dispose();
                    this.dbContext = null;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }

}
