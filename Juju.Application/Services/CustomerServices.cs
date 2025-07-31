using AutoMapper;
using FluentValidation;
using Juju.Application.Contracts;
using Juju.Application.Contracts.Services;
using Juju.Application.Dtos;
using Juju.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Juju.Application.Services
{
    public class CustomerServices : BaseServices, ICustomerServices
    {
        
        private readonly IValidator<CustomerRequest> _validator;

        public CustomerServices(IValidator<CustomerRequest> validator, IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _validator = validator;
        }

        public Task<HttpResponse<bool>> CreateCustomer(CustomerRequest entity)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponse<bool>> DeleteCustomer(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<HttpResponse<List<CustomerDto>>> GetAll()
        {
            try
            {
                
                IReadOnlyList<Domain.Entities.Customer> customers = await _unitOfWork.CustomerRepository.GetAllAsync();

                List<CustomerDto> customerDtos = _mapper.Map<List<CustomerDto>>(customers);

                return new HttpResponse<List<CustomerDto>>
                {
                    Data = customerDtos,
                    HttpStatusCode = HttpStatusCode.OK,
                    Message = "Clientes obtenidos correctamente."
                };
            }
            catch (Exception ex)
            {
                return new HttpResponse<List<CustomerDto>>
                {
                    Message = $"Error interno: {ex.Message}",
                    HttpStatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public Task<HttpResponse<CustomerDto>> UpdateCustomer(CustomerDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
