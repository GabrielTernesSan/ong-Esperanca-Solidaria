using MediatR;
using Ong.Application.Requests;
using Ong.Domain.Repositories.UnitOfWork;
using System.Text.Json;

namespace Ong.Application.Handlers
{
    public class ProcessOutboxHandler : IRequestHandler<ProcessOutboxRequest>
    {
        private readonly IOutboxMessageRepository _outboxRepository;
        private readonly IUnitOfWork _unitRepository;
        private readonly IMediator _mediator;

        public ProcessOutboxCommandHandler(IOutboxMessageRepository outboxRepository, IMediator mediator, IUnitOfWork unitRepository)
        {
            _outboxRepository = repository;
            _mediator = mediator;
            _unitRepository = unitRepository;
        }

        public async Task Handle(ProcessOutboxCommand request, CancellationToken cancellationToken)
        {
            var messages = await _outboxRepository.GetUnprocessedAsync();

            foreach (var message in messages)
            {
                try
                {
                    var type = Type.GetType(message.Type);
                    var payload = JsonSerializer.Deserialize(message.Payload, type!);

                    await _mediator.Publish(payload!, cancellationToken);

                    message.ProcessedOn = DateTime.UtcNow;
                }
                catch (Exception ex)
                {
                    message.Error = ex.Message;
                }
            }

            await _unitRepository.CommitAsync(cancellationToken);
        }
    }
}
