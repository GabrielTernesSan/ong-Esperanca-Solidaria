namespace Ong.Domain.Repositories.UnitOfWork
{
    public interface IOutboxMessageRepository
    {
        Task CreateAsync(OutboxMessage message);
    }
}
