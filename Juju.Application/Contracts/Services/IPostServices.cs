using Juju.Application.Dtos;
using Juju.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juju.Application.Contracts.Services
{
    public interface IPostServices
    {
        Task<HttpResponse<List<PostDto>>> GetAll();
        Task<HttpResponse<bool>> CreatePost(PostDto entity);
        Task<HttpResponse<PostDto>> UpdatePost(PostDto entity);
        Task<HttpResponse<bool>> DeletePost(long id);
    }
}
