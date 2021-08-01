using System.Linq;
using System.Threading.Tasks;
using UnofficialDevryIT.Architecture.Models;

namespace UnofficialDevryIT.Architecture.Extensions
{
    public static class PaginatedListExtensions
    {
        public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(
            this IQueryable<TDestination> queryable, int pageNumber, int pageSize)
            => PaginatedList<TDestination>.CreateAsync(queryable, pageNumber, pageSize);
    }
}