using Juju.Application.Contracts.Services;
using Juju.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Customer
{
    public class CustomerController : BaseController
    {
        private readonly ICustomerServices _customerServices;
        public CustomerController(ICustomerServices customerService)
        {
            _customerServices = customerService;
        }

        [HttpGet("GetCustomers")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _customerServices.GetAll();
            return StatusCode((int)result.HttpStatusCode, result);
        }


        [HttpPost("CreateCustomer")]
        public async Task<IActionResult> Create([FromBody] CustomerRequest entity)
        {
            var result = await _customerServices.CreateCustomer(entity);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        [HttpPut("UpdateCustomer")]
        public async Task<IActionResult> Update(CustomerDto entity)
        {
            var result = await _customerServices.UpdateCustomer(entity);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        [HttpDelete("DeleteCustomer/{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var result = await _customerServices.DeleteCustomer(Id);
            return StatusCode((int)result.HttpStatusCode, result);
        }
    }   
}
