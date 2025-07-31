using Juju.Application.Contracts;
using Juju.Infrastructure.Persistence;
using Juju.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juju.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly JujuTestContext _context;
        private IDbContextTransaction _transaction;

        public ICustomerRepository customerRepository { get; private set; }
        public IPostRepository postRepository { get; private set; }

        public UnitOfWork(JujuTestContext jujuTestContext)
        {
            _context = jujuTestContext;
            customerRepository = new CustomerRepository(_context);
            postRepository = new PostRepository(_context);
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CompleteAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
        }
    }
}
