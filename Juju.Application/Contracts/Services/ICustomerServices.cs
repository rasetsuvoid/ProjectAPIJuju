using Juju.Application.Dtos;
using Juju.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juju.Application.Contracts.Services
{
    public interface ICustomerServices
    {
        Task<HttpResponse<List<CustomerDto>>> GetAll();
        Task<HttpResponse<bool>> CreateCustomer(CustomerRequest entity);
        Task<HttpResponse<CustomerDto>> UpdateCustomer(CustomerDto entity);
        Task<HttpResponse<bool>> DeleteCustomer(int id);
    }
}