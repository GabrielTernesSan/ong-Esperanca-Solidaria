namespace Ong.Domain
{
    public class Donation
    {
        public Guid Id { get; }
        public Guid CampaignId { get; private set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }

        public Donation(Guid campaignId, Guid userId, decimal amount)
        {
            CampaignId = campaignId;
            UserId = userId;
            Amount = amount;
        }

        public Donation(Guid id, Guid campaignId, Guid userId, decimal amount)
            : this(campaignId, userId, amount)
        {
            Id = id;
        }
    }
}
