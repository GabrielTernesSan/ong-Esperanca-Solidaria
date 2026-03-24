using Hangfire;
using MediatR;
using Ong.Application.Requests;
using Ong.Application.Worker.Configurations;
using System.ComponentModel;

namespace Ong.Application.Worker.Jobs
{
    public class OutboxPublisherJob
    {
        private readonly IMediator _mediator;

        public OutboxPublisherJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Queue(Queues.OutboxPublisherJob)]
        [DisplayName("outbox-publisher-job")]
        public async Task ProcessAsync()
        {
            await _mediator.Send(new ProcessOutboxRequest { });
        }
    }
}
