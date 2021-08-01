namespace UnofficialDevryIT.Architecture.Models
{
    /// <summary>
    /// Basic implementation for domain events
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public abstract class BaseEvent<TDomainObject> : DomainEvent
    {
        public TDomainObject Result { get; }

        public BaseEvent(TDomainObject result)
        {
            Result = result;
        }
    }
}