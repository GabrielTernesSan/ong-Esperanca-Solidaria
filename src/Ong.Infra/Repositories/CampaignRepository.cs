using Ong.Domain;
using Ong.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ong.Infra.Repositories
{
    public class CampaignRepository : ICampaignRepository
    {
        private readonly OngDbContext _context;

        public CampaignRepository(OngDbContext context)
        {
            _context = context;
        }

        public async Task<Campaign?> GetByIdAsync(Guid id)
        {
            var entity = await _context.Campaigns.FindAsync(id);

            return new Campaign(
                entity.Id,
                entity.Title,
                entity.Description,
                entity.StartDate,
                entity.EndDate,
                entity.FinancialGoal,
                entity.Status,
                entity.CurrentAmount);
        }

        public async Task UpdateAsync(Campaign campaign)
        {
            var entity = await _context.Campaigns.FindAsync(campaign.Id);

            if (entity == null) return;

            entity.Title = campaign.Title;
            entity.Description = campaign.Description;
            entity.StartDate = campaign.StartDate;
            entity.EndDate = campaign.EndDate;
            entity.FinancialGoal = campaign.FinancialGoal;
            entity.CurrentAmount = campaign.CurrentAmount;
            entity.Status = campaign.Status;

            _context.Campaigns.Update(entity);

            await _context.SaveChangesAsync();
        }
    }
}
