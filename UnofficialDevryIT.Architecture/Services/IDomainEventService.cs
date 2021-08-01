using System.Threading.Tasks;
using UnofficialDevryIT.Architecture.Models;

namespace UnofficialDevryIT.Architecture.Services
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}