using System.Linq.Expressions;
using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces.Repository;

public interface IPostRepository : IBaseRepository<Post>
{
    public IAsyncEnumerable<Post> GetSortedPostsByRating(Expression<Func<Post, bool>> predicate, int skip = 0, int take = int.MaxValue);
}