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
    public class PostRequestValidator : AbstractValidator<PostRequest>
    {
        public PostRequestValidator(ICustomerRepository customerRepository)
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("El título es obligatorio.")
                .MinimumLength(5).WithMessage("El título debe tener al menos 5 caracteres.")
                .MaximumLength(100).WithMessage("El título no debe exceder los 100 caracteres.");

            RuleFor(x => x.Body)
                .NotEmpty().WithMessage("El cuerpo es obligatorio.")
                .MaximumLength(500).WithMessage("El cuerpo no debe exceder los 500 caracteres.");

            RuleFor(x => x.Type)
               .GreaterThan(0).WithMessage("El tipo debe ser mayor a 0.");

            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage("Debe especificar un cliente válido.")
                .MustAsync(async (id, cancellation) =>
                {
                    var exists = await customerRepository.GetByIdAsync(x => x.CustomerId == id);
                    return !object.Equals(exists, null);
                }).WithMessage("El cliente especificado no existe.");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("La categoría es obligatoria si el tipo no es 1, 2 o 3.")
                .When(x => x.Type != 1 && x.Type != 2 && x.Type != 3);
        }
    }
}