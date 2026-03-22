namespace Ong.Domain.Repositories.UnitOfWork
{
    public interface IOutboxRepository
    {
        Task CreateAsync(OutboxMessage message);
    }
}
