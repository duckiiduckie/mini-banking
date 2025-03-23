using System;
using System.Threading.Tasks;
using Banking.Core.Domain;

namespace Banking.Core.Repositories
{
    public interface IAccountRepository
    {
        Task<Account> GetById(Guid id);
        Task<Account> GetByUserId(Guid userId);
        Task Add(Account account);
        Task Update(Account account);
    }
} 