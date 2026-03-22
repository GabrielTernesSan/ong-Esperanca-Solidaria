namespace Ong.Commom
{
    public record DonationCreated(Guid DonationId, Guid UserId, Guid CampaignId, decimal Amount, DateTime Timestamp);
}
