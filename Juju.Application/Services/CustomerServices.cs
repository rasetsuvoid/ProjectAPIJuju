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
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Juju.Application.Services
{
    public class CustomerServices : BaseServices, ICustomerServices
    {
        
        private readonly IValidator<CustomerRequest> _validator;
        private readonly IValidator<CustomerDto> _updateValidator;

        public CustomerServices(IValidator<CustomerRequest> validator, IUnitOfWork unitOfWork, IMapper mapper, IValidator<CustomerDto> validatorUpdate) : base(unitOfWork, mapper)
        {
            _validator = validator;
            _updateValidator = validatorUpdate;
        }

        public async Task<HttpResponse<bool>> CreateCustomer(CustomerRequest entity)
        {
            var validationResult = await _validator.ValidateAsync(entity);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                return HttpResponse<bool>.Fail(HttpStatusCode.BadRequest, $"Error de validación: {errors}");
            }

            var customer = _mapper.Map<Customer>(entity);

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.customerRepository.AddAsync(customer);
                await _unitOfWork.CommitAsync();

                return HttpResponse<bool>.Success(HttpStatusCode.Created, "Cliente creado correctamente.", true);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return HttpResponse<bool>.Fail(HttpStatusCode.InternalServerError, $"Error durante el guardado: {ex.Message}");
            }
        }


        public async Task<HttpResponse<bool>> DeleteCustomer(int id)
        {
            try
            {
                Customer customer = await GetCustomerWithPostsByIdAsync(id);

                if (customer == null)
                    return HttpResponse<bool>.Fail(HttpStatusCode.NotFound, $"No se encontró el cliente con Id {id}.");

                await DeleteWithPostsAsync(customer);

                return HttpResponse<bool>.Success(
                    HttpStatusCode.OK,
                    "Cliente y posts eliminados correctamente.",
                    true);
            }
            catch (Exception ex)
            {
                return HttpResponse<bool>.Fail(
                    HttpStatusCode.InternalServerError,
                    $"Error interno: {ex.Message}");
            }
        }


        public async Task<HttpResponse<List<CustomerDto>>> GetAll()
        {

            try
            {
                IReadOnlyList<Customer> customers = await _unitOfWork.customerRepository.GetAllAsync(x => x.Active);

                List<CustomerDto> customerDtos = _mapper.Map<List<CustomerDto>>(customers);

                return HttpResponse<List<CustomerDto>>.Success(
                    HttpStatusCode.OK,
                    "Clientes obtenidos correctamente.",
                    customerDtos);
            }
            catch (Exception ex)
            {
                return HttpResponse<List<CustomerDto>>.Fail(
                    HttpStatusCode.InternalServerError,
                    $"Error interno: {ex.Message}");
            }
        }


        public async Task<HttpResponse<CustomerDto>> UpdateCustomer(CustomerDto entity)
        {
            try
            {
                FluentValidation.Results.ValidationResult validationResult = await _updateValidator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return HttpResponse<CustomerDto>.Fail(HttpStatusCode.BadRequest, $"Error de validación: {errors}");
                }

                Customer customer = await GetCustomerWithPostsByIdAsync(entity.CustomerId);
                if (customer == null)
                {
                    return HttpResponse<CustomerDto>.Fail(HttpStatusCode.NotFound, $"No se encontró el cliente con Id {entity.CustomerId}.");
                }

                try
                {

                    customer.Name = entity.Name;

                    await _unitOfWork.BeginTransactionAsync();
                    await _unitOfWork.customerRepository.UpdatePartialAsync(customer);
                    await _unitOfWork.CommitAsync();

                    CustomerDto dto = _mapper.Map<CustomerDto>(customer);

                    return HttpResponse<CustomerDto>.Success(HttpStatusCode.OK, "Cliente actualizado correctamente.", dto);
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackAsync();
                    return HttpResponse<CustomerDto>.Fail(HttpStatusCode.InternalServerError, $"Error durante el guardado: {ex.Message}");
                }

            }
            catch (Exception ex)
            {
                return HttpResponse<CustomerDto>.Fail(HttpStatusCode.InternalServerError, $"Error interno: {ex.Message}");
            }
        }


        #region Metodos Auxiliares


        public async Task<Customer> GetCustomerWithPostsByIdAsync(int id)
        {
            return await _unitOfWork.customerRepository.GetByIdWithIncludeAsync(c => c.CustomerId == id && c.Active, c => c.Posts);
        }

        public async Task DeleteWithPostsAsync(Customer customer)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                if (customer.Posts?.Any() == true)
                {
                    await _unitOfWork.postRepository.RemoveRange(customer.Posts);
                }

                await _unitOfWork.customerRepository.Remove(customer);

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
