using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Banking.Core.Domain;
using Banking.Core.Repositories;
using Banking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Banking.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly BankingDbContext _context;

        public TransactionRepository(BankingDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction> GetById(Guid id)
        {
            return await _context.Transactions.FindAsync(id);
        }

        public async Task<IEnumerable<Transaction>> GetByCustomerId(Guid customerId, DateTime? fromDate, DateTime? toDate, int page, int pageSize)
        {
            var query = _context.Transactions.Where(t => t.CustomerId == customerId);

            if (fromDate.HasValue)
                query = query.Where(t => t.CreatedAt >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(t => t.CreatedAt <= toDate.Value);

            query = query.OrderByDescending(t => t.CreatedAt)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task Add(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
        }
    }
} 