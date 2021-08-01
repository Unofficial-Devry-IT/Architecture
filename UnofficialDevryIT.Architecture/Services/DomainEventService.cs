using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using UnofficialDevryIT.Architecture.Models;

namespace UnofficialDevryIT.Architecture.Services
{
    public class DomainEventService : IDomainEventService
    {
        private readonly ILogger<DomainEventService> _logger;
        private readonly IPublisher _mediator;

        public DomainEventService(IPublisher mediator, ILogger<DomainEventService> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Publish(DomainEvent domainEvent)
        {
            _logger.LogInformation($"Publishing domain event. Event - {domainEvent.GetType().Name}");
            await _mediator.Publish(GetNotificationCorrespondingToDomainEvent(domainEvent));
        }

        private INotification GetNotificationCorrespondingToDomainEvent(DomainEvent domainEvent)
        {
            return (INotification) Activator.CreateInstance(
                typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType()), domainEvent);
        }
    }
}