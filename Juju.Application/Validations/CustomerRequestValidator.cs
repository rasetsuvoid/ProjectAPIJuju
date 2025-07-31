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
    internal class CustomerValidator : AbstractValidator<CustomerRequest>
    {
        public CustomerValidator(ICustomerRepository customerRepository)
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("El nombre no puede ser nulo.")
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres.")
                .MaximumLength(50).WithMessage("El nombre no debe exceder los 50 caracteres.")
                .MustAsync(async (name, cancellation) =>
                {
                    var exists = await customerRepository.ExistsByNameAsync(name);
                    return !exists;
                })
                .WithMessage("Ya existe un usuario con ese nombre.");
        }
    }
}
