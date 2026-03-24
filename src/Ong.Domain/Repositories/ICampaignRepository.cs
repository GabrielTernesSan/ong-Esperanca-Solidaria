using System;
using System.Collections.Generic;
using System.Text;

namespace Ong.Domain.Repositories
{
    public interface ICampaignRepository
    {
        Task<Campaign?> GetByIdAsync(Guid id);
        Task UpdateAsync(Campaign campaign);
    }
}
