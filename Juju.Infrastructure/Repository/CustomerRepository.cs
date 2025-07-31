using Juju.Application.Contracts;
using Juju.Domain.Entities;
using Juju.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juju.Infrastructure.Repository
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(JujuTestContext context) : base(context)
        {
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Customer.AnyAsync(c => c.Name == name);
        }

        public async Task<Customer> GetByNameAsync(string name)
        {
            return await _context.Customer
                .FirstOrDefaultAsync(c => c.Name == name && !c.IsDeleted);
        }
    }
}
