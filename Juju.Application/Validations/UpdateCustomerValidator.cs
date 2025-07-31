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
    public class UpdateCustomerValidator : AbstractValidator<CustomerDto>
    {
        public UpdateCustomerValidator(ICustomerRepository customerRepository)
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage("El ID del cliente debe ser mayor que cero.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres.")
                .MaximumLength(50).WithMessage("El nombre no debe exceder los 50 caracteres.")
                .MustAsync(async (dto, name, cancellation) =>
                {
                    var existing = await customerRepository.GetByNameAsync(name);
                    return existing == null || existing.CustomerId == dto.CustomerId;
                }).WithMessage("Ya existe otro cliente con ese nombre.");

        }
    }
}
