using System.Data;

namespace ZeroFrame.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        // Define um contrato para executar operações dentro de uma transação.
        Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> operation, IsolationLevel isolation = IsolationLevel.ReadCommitted);
        Task ExecuteInTransactionAsync(Func<Task> operation);
    }
}
