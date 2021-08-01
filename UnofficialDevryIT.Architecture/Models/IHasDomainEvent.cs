using System.Collections.Generic;

namespace UnofficialDevryIT.Architecture.Models
{
    public interface IHasDomainEvent
    {
        public List<DomainEvent> DomainEvents { get; set; }
    }
}