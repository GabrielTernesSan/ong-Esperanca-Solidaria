using Microsoft.EntityFrameworkCore;
using Ong.Domain;
using Ong.Domain.Repositories;

namespace Ong.Infra.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly OngDbContext _context;

        public UserRepository(OngDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .AsNoTracking()
                .Select(u => new User(u.Id, u.Name, u.Email, u.PasswordHash, u.Role))
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task CreateAsync(User user)
        {
            var entity = new Tables.User
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                Role = user.Role
            };

            _context.Users.Add(entity);

            await _context.SaveChangesAsync();
        }
    }
}
