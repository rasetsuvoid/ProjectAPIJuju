using FluentValidation;
using Juju.Application.Contracts;
using Juju.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juju.Application.Validations
{
    public class UpdatePostValidator : AbstractValidator<PostDto>
    {
        public UpdatePostValidator(ICustomerRepository customerRepository)
        {
             
        }
    }
}
