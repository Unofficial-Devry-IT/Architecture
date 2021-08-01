namespace UnofficialDevryIT.Architecture.Models
{
    public interface IEntityWithTypedId<TId>
    {
        TId Id { get; }
    }
}