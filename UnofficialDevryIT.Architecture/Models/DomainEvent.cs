using System;

namespace UnofficialDevryIT.Architecture.Models
{
    public abstract class DomainEvent
    {
        public DateTimeOffset DateOccurred { get; protected set; } = DateTimeOffset.Now;
        public bool IsPublished { get; set; }
    }
}