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
        private readonly IValidator<PostUpdate> _updateValidator;

        public PostServices(IUnitOfWork unitOfWork, IMapper mapper, IValidator<PostUpdate> updateValidator, IValidator<PostRequest> postValidator) : base(unitOfWork, mapper)
        {
            _updateValidator = updateValidator;
            _postValidator = postValidator;
        }

        public async Task<HttpResponse<bool>> CreateManyPosts(List<PostRequest> posts)
        {

            List<Post> postList = [];

            foreach (PostRequest item in posts)
            {
                var validationResult = await _postValidator.ValidateAsync(item);

                if (!validationResult.IsValid)
                {
                    var errors = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return HttpResponse<bool>.Fail(HttpStatusCode.BadRequest, $"Error de validación: {errors}");
                }

                Post post = Create(item);
                postList.Add(post);
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.postRepository.AddRangeAsync(postList);
                await _unitOfWork.CommitAsync();

                return HttpResponse<bool>.Success(HttpStatusCode.Created, "Post creado correctamente.", true);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return HttpResponse<bool>.Fail(HttpStatusCode.InternalServerError, $"Error durante el guardado: {ex.Message}");
            }
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

        public async Task<HttpResponse<PostDto>> UpdatePost(PostUpdate entity)
        {
            try
            {
                FluentValidation.Results.ValidationResult validationResult = await _updateValidator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return HttpResponse<PostDto>.Fail(HttpStatusCode.BadRequest, $"Error de validación: {errors}");
                }

                Post post = await GetPostByIdAsync(entity.PostId);
                if (post == null)
                {
                    return HttpResponse<PostDto>.Fail(HttpStatusCode.NotFound, $"No se encontró el post con Id {entity.PostId}.");
                }

                try
                {
                    post.Title = entity.Title;
                    post.Type = entity.Type;
                    post.Body = entity.Body.Length > 20
                        ? entity.Body.EndsWith("...")
                            ? entity.Body
                            : entity.Body.Substring(0, Math.Min(97, entity.Body.Length)) + "..."
                        : entity.Body;
                    post.Category = post.Type switch
                    {
                        1 => "Farándula",
                        2 => "Política",
                        3 => "Futbol",
                        _ => post.Category
                    };
                    post.CustomerId = entity.CustomerId;

                    await _unitOfWork.BeginTransactionAsync();
                    await _unitOfWork.postRepository.UpdatePartialAsync(post);
                    await _unitOfWork.CommitAsync();

                    PostDto dto = _mapper.Map<PostDto>(post);

                    return HttpResponse<PostDto>.Success(HttpStatusCode.OK, "Post actualizado correctamente.", dto);
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackAsync();
                    return HttpResponse<PostDto>.Fail(HttpStatusCode.InternalServerError, $"Error durante el guardado: {ex.Message}");
                }

            }
            catch (Exception ex)
            {
                return HttpResponse<PostDto>.Fail(HttpStatusCode.InternalServerError, $"Error interno: {ex.Message}");
            }
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
