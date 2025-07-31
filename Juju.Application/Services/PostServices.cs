using Juju.Application.Contracts.Services;
using Juju.Application.Dtos;
using Juju.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juju.Application.Services
{
    public class PostServices : IPostServices
    {
        public Task<HttpResponse<bool>> CreatePost(PostDto entity)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponse<bool>> DeletePost(long id)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponse<List<PostDto>>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponse<PostDto>> UpdatePost(PostDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
