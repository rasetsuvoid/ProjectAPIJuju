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
    public class CustomerValidator : AbstractValidator<CustomerRequest>
    {
        public CustomerValidator(ICustomerRepository customerRepository)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MustAsync(async (name, cancellation) =>
                {
                    var exists = await customerRepository.ExistsByNameAsync(name);
                    return !exists;
                })
                .WithMessage("Ya existe un usuario con ese nombre.");
        }
    }
}
