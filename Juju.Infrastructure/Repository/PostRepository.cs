using Juju.Application.Contracts;
using Juju.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juju.Infrastructure.Repository
{
    public class PostRepository : Repository<Domain.Entities.Post>, IPostRepository
    {
        public PostRepository(JujuTestContext context) : base(context)
        {
        }
    }
}
