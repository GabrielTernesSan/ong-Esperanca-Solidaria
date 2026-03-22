namespace Ong.Domain
{
    public record OutboxMessage(Guid Id, string Type, string Payload, DateTime OccurredOn, DateTime? ProcessedOn = null, string? Error = null);
}
