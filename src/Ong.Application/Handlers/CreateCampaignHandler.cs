using MediatR;
using Ong.Application.Requests;
using Ong.Commom;
using Ong.Domain;
using Ong.Domain.Repositories;
using Ong.Domain.Repositories.UnitOfWork;

namespace Ong.Application.Handlers
{
    public class CreateCampaignHandler : IRequestHandler<CreateCampaignRequest, Response>
    {
        private readonly ICampaignRepository _campaignRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCampaignHandler(
            ICampaignRepository campaignRepository,
            IUnitOfWork unitOfWork)
        {
            _campaignRepository = campaignRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(CreateCampaignRequest request, CancellationToken cancellationToken)
        {
            var response = new Response();

            var campaign = Campaign.Create(
                request.Title,
                request.Description,
                request.StartDate,
                request.EndDate,
                request.FinancialGoal
            );

            await _campaignRepository.AddAsync(campaign);

            response.SetResult(campaign.Id);

            return response;
        }
    }
}
