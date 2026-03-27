using MediatR;
using Ong.Application.Requests;
using Ong.Commom;
using Ong.Domain.Repositories;

namespace Ong.Application.Handlers
{
    public class GetActiveCampaignsHandler : IRequestHandler<GetActiveCampaignsRequest, Response>
    {
        private readonly ICampaignRepository _campaignRepository;

        public GetActiveCampaignsHandler(ICampaignRepository campaignRepository)
        {
            _campaignRepository = campaignRepository;
        }

        public async Task<Response> Handle(GetActiveCampaignsRequest request, CancellationToken cancellationToken)
        {
            var response = new Response();

            var campaigns = await _campaignRepository.GetActiveAsync();

            response.SetResult(campaigns.Select(c => new
            {
                c.Id,
                c.Title,
                c.FinancialGoal,
                c.CurrentAmount
            }));

            return response;
        }
    }
}