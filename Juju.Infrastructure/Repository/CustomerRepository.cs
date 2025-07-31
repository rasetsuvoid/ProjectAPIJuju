using DataAccess.Data;
using Juju.Application.Contracts;
using Juju.Domain.Entities;
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
    }
}
