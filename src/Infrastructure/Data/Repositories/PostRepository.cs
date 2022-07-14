using System.Linq.Expressions;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repository;

namespace Infrastructure.Data.Repositories;

public class PostRepository : BaseRepository<Post>, IPostRepository
{
    private readonly MarsContext _dbContext;

    public PostRepository(MarsContext marsContext) : base(marsContext)
    {
        _dbContext = marsContext;
    }

    public IAsyncEnumerable<Post> GetSortedPostsByRating(Expression<Func<Post, bool>> predicate, int skip = 0,
        int take = int.MaxValue)
    {
        return _dbContext.Posts
            .Where(predicate)
            .GroupJoin(_dbContext.PostRatings, post => post.Id, rating => rating.PostId, 
                (post, ratings) => new { post, ratings })
            .SelectMany(x => x.ratings.DefaultIfEmpty(), 
                (x, rating) => new { x.post, rating })
            .GroupBy(x => x.post.Id)
            .Select(x => new
            {
                x.Single().post,
                rating = x.Where(z => z.rating != null).Sum(z => z.rating!.IsLike ? 1 : -1)
            })
            .OrderByDescending(x => x.rating)
            .Skip(skip).Take(take)
            .ToAsyncEnumerable()
            .Select(x => x.post);
    }
}