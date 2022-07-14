using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class RatingRepository : BaseRepository<PostRating>, IRatingRepository
{
    private readonly MarsContext _dbContext;

    public RatingRepository(MarsContext marsContext) : base(marsContext)
    {
        _dbContext = marsContext;
    }

    public async Task<int> GetSumPostRating(int postId)
    {
        return await _dbContext.PostRatings.Where(x => x.PostId == postId).SumAsync(x => x.IsLike ? 1 : -1);
    }

    public async Task<int> GetSumUserRating(int userId)
    {
        return await _dbContext.Posts
            .Join(_dbContext.PostRatings, post => post.Id, rating => rating.PostId, (post, rating) => new
            {
                postUserId = post.UserId,
                isLike = rating.IsLike
            })
            .Where(x => x.postUserId == userId)
            .SumAsync(x => x.isLike ? 1 : -1);
    }
}