using AutoMapper;
using FluentValidation;
using Juju.Application.Contracts;
using Juju.Application.Contracts.Services;
using Juju.Application.Dtos;
using Juju.Domain.Entities;
using Juju.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Juju.Application.Services
{
    public class PostServices : BaseServices, IPostServices
    {
        private readonly IValidator<PostRequest> _postValidator;
        private readonly IValidator<PostDto> _updateValidator;

        public PostServices(IUnitOfWork unitOfWork, IMapper mapper, IValidator<PostDto> updateValidator, IValidator<PostRequest> postValidator) : base(unitOfWork, mapper)
        {
            _updateValidator = updateValidator;
            _postValidator = postValidator;
        }

        public Task<HttpResponse<bool>> CreateManyPosts(List<PostDto> posts)
        {
            throw new NotImplementedException();
        }

        public async Task<HttpResponse<bool>> CreatePost(PostRequest entity)
        {
            var validationResult = await _postValidator.ValidateAsync(entity);

            if (!validationResult.IsValid)
            {
                var errors = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                return HttpResponse<bool>.Fail(HttpStatusCode.BadRequest, $"Error de validación: {errors}");
            }

            Post post = Create(entity);

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.postRepository.AddAsync(post);
                await _unitOfWork.CommitAsync();

                return HttpResponse<bool>.Success(HttpStatusCode.Created, "Post creado correctamente.", true);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return HttpResponse<bool>.Fail(HttpStatusCode.InternalServerError, $"Error durante el guardado: {ex.Message}");
            }

        }

        public async Task<HttpResponse<bool>> DeletePost(int id)
        {
            try
            {
                Post post = await GetPostByIdAsync(id);

                if (post == null)
                    return HttpResponse<bool>.Fail(HttpStatusCode.NotFound, $"No se encontró el post con Id {id}.");

                await DeleteAsync(post);

                return HttpResponse<bool>.Success(
                    HttpStatusCode.OK,
                    "Post eliminados correctamente.",
                    true);
            }
            catch (Exception ex)
            {
                return HttpResponse<bool>.Fail(
                    HttpStatusCode.InternalServerError,
                    $"Error interno: {ex.Message}");
            }
        }

        public async Task<HttpResponse<List<PostDto>>> GetAll()
        {
            try
            {
                IReadOnlyList<Post> posts = await _unitOfWork.postRepository.GetAllAsync(x => x.Active, x => x.Customer);

                List<PostDto> postDtos = _mapper.Map<List<PostDto>>(posts);

                return HttpResponse<List<PostDto>>.Success(
                    HttpStatusCode.OK,
                    "Post obtenidos correctamente.",
                    postDtos);
            }
            catch (Exception ex)
            {
                return HttpResponse<List<PostDto>>.Fail(
                    HttpStatusCode.InternalServerError,
                    $"Error interno: {ex.Message}");
            }
        }

        public Task<HttpResponse<PostDto>> UpdatePost(PostDto entity)
        {
            throw new NotImplementedException();
        }

        #region Generic

        public Post Create(PostRequest request)
        {
            var post = _mapper.Map<Post>(request);

            // Truncar Body
            if (post.Body.Length > 20)
                post.Body = post.Body.Substring(0, Math.Min(97, post.Body.Length)) + "...";

            // Asignar Category según Type
            post.Category = post.Type switch
            {
                1 => "Farándula",
                2 => "Política",
                3 => "Futbol",
                _ => post.Category
            };

            return post;
        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            return await _unitOfWork.postRepository.GetByIdWithIncludeAsync(c => c.PostId == id && c.Active);
        }

        public async Task DeleteAsync(Post post)
        {
            

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                await _unitOfWork.postRepository.Remove(post);

                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
        #endregion
    }
}
