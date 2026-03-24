using MediatR;
using Ong.Application.Requests;
using Ong.Commom;
using Ong.Domain;
using Ong.Domain.Repositories.UnitOfWork;
using System.Text.Json;

namespace Ong.Application.Handlers
{
    public class DonationHandler : IRequestHandler<DonationRequest, Response>
    {
        private readonly IDonationRepository _donationRepository;
        private readonly IOutboxMessageRepository _outboxRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DonationHandler(IDonationRepository donationRepository, IOutboxMessageRepository outboxRepository, IUnitOfWork unitOfWork)
        {
            _donationRepository = donationRepository;
            _outboxRepository = outboxRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(DonationRequest request, CancellationToken cancellationToken)
        {
            var response = new Response();

            var donationId = Guid.NewGuid();

            var donation = new Donation(
                donationId,
                request.CampaignId,
                request.UserId,
                request.Amount,
                DateTimeOffset.Now
            );

            var donationEvent = new DonationCreated(
                donationId,
                request.UserId,
                request.CampaignId,
                request.Amount,
                DateTime.UtcNow
            );

            var outboxMessage = new OutboxMessage(
                Guid.NewGuid(),
                typeof(DonationCreated).Name!,
                JsonSerializer.Serialize(donationEvent),
                DateTime.UtcNow
            );

            await _donationRepository.CreateAsync(donation);

            await _outboxRepository.CreateAsync(outboxMessage);

            await _unitOfWork.CommitAsync(cancellationToken);

            return response;
        }
    }
}
