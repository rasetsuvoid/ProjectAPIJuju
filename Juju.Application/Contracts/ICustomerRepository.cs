using Juju.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juju.Application.Contracts
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<bool> ExistsByNameAsync(string name);
        Task<Customer> GetByNameAsync(string name);

    }
}
