using SellerCloud.BusinessRules.DAL.Repositories;
using SellerCloud.BusinessRules.DAL.Services;
using System;
using System.Threading.Tasks;

namespace SellerCloud.BusinessRules.DAL
{
    public interface IBusinessRulesEngineUnitOfWork : IDisposable
    {
        IRuleModuleRepository RuleModuleRepository { get; }
        IRuleRepository RuleRepository { get; }
        IRuleArgumentRepository RuleArgumentRepository { get; }

        int ExecuteSqlCommand(string sqlCommand);
        int Commit();
        Task<int> CommitAsync();
    }
}
