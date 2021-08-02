using UnofficialDevryIT.Architecture.Models;

namespace UnofficialDevryIT.Architecture.Data
{
    public interface IRepository<T> : IRepositoryWithTypedId<T, ulong> where T : IEntityWithTypedId<ulong>
    {
        
    }
}