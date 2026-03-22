using Ong.Domain;
using Ong.Domain.Repositories.UnitOfWork;

namespace Ong.Infra.Repositories.UnitOfWork
{
    public class DonationRepository : IDonationRepository
    {
        private readonly OngDbContext _context;

        public DonationRepository(OngDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Donation donation)
        {
            var entity = new Tables.Donation
            {
                Id = donation.Id,
                CampaignId = donation.CampaignId,
                UserId = donation.UserId,
                Amount = donation.Amount,
                Date = donation.Date
            };

            await _context.Donations.AddAsync(entity);
        }
    }
}
