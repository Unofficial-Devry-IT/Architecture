using Microsoft.EntityFrameworkCore;

namespace UnofficialDevryIT.Architecture.Data
{
    public interface ICustomModelBuilder
    {
        void Build(ModelBuilder modelBuilder);
    }
}