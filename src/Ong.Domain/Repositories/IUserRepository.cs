namespace Ong.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByCpfAsync(string normalizedCpf);
        Task CreateAsync(User user);
    }
}
