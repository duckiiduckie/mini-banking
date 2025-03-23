using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Banking.Core.Domain;

namespace Banking.Core.Repositories
{
    public interface ITransactionRepository
    {
        Task<Domain.Transaction> GetById(Guid id);
        Task<IEnumerable<Transaction>> GetByCustomerId(Guid customerId, DateTime? fromDate, DateTime? toDate, int page, int pageSize);
        Task Add(Transaction transaction);
        Task Update(Transaction transaction);
    }
} 