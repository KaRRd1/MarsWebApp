using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces.Repository;

public interface IRatingRepository : IBaseRepository<PostRating>
{
    Task<int> GetSumPostRating(int postId);
    Task<int> GetSumUserRating(int userId);
}