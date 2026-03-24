namespace Ong.Domain.Repositories.UnitOfWork
{
    public interface IDonationRepository
    {
        Task<Donation?> GetByIdAsync(Guid id);
        Task CreateAsync(Donation donation);
    }
}
