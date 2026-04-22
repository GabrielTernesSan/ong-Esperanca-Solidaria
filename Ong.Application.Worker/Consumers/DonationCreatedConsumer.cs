using MassTransit;
using Microsoft.Extensions.Logging;
using Ong.Commom;
using Ong.Domain.Enums;
using Ong.Domain.Repositories;
using Ong.Domain.Repositories.UnitOfWork;

namespace Ong.Application.Worker.Consumers
{
    public class DonationCreatedConsumer : IConsumer<DonationCreated>
    {
        private readonly ILogger<DonationCreatedConsumer> _logger;
        private readonly ICampaignRepository _campaignRepository;
        private readonly IDonationRepository _donationRepository;
        private readonly IUserRepository _userRepository;

        public DonationCreatedConsumer(ILogger<DonationCreatedConsumer> logger, ICampaignRepository campaignRepository, IDonationRepository donationRepository, IUserRepository userRepository)
        {
            _logger = logger;
            _campaignRepository = campaignRepository;
            _donationRepository = donationRepository;
            _userRepository = userRepository;
        }

        public async Task Consume(ConsumeContext<DonationCreated> context)
        {
            var evento = context.Message;

            _logger.LogInformation($"Processando doação: {evento.DonationId} no valor de {evento.Amount}");

            var donation = await _donationRepository.GetByIdAsync(evento.DonationId);

            if (donation == null)
            {
                _logger.LogError($"Doação com ID {evento.DonationId} não encontrada.");
                return;
            }

            var user = await _userRepository.GetByIdAsync(evento.UserId);

            if (user == null)
            {
                _logger.LogError($"Usuário com ID {evento.UserId} não encontrado para doação {evento.DonationId}.");
                return;
            }

            var campaign = await _campaignRepository.GetByIdAsync(evento.CampaignId);

            if (campaign == null)
            {
                _logger.LogError($"Campanha com ID {evento.CampaignId} não encontrada para doação {evento.DonationId}.");
                return;
            }

            if (campaign.Status == ECampaignStatus.Canceled || campaign.Status == ECampaignStatus.Completed)
                return;

            var totalAmount = await _donationRepository.GetTotalAmountByCampaignIdAsync(evento.CampaignId);

            if (totalAmount < campaign.FinancialGoal)
                return;

            campaign.MarkAsCompleted();

            await _campaignRepository.UpdateAsync(campaign);
        }
    }
}
