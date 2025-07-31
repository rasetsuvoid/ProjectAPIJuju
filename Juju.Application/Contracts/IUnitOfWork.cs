using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juju.Application.Contracts
{
    public interface IUnitOfWork
    {
        Task BeginTransactionAsync(); // Inicia una nueva transacción
        Task CommitAsync();           // Confirma los cambios realizados en la transacción
        Task CompleteAsync();         // Guarda cambios y confirma la transacción
        Task RollbackAsync();         // Revierte la transacción actual

        ICustomerRepository customerRepository { get; }
        IPostRepository postRepository { get; }
    }
}
