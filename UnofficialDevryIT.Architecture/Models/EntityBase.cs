using System;

namespace UnofficialDevryIT.Architecture.Models
{
    public class EntityBase : EntityWithTypedId<Guid>
    {
        protected EntityBase()
        {
            Id = Guid.NewGuid();
        }
    }
}