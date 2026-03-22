namespace Ong.Domain.Repositories.UnitOfWork
{
    public interface IDonationRepository
    {
        Task CreateAsync(Donation donation);
    }
}
