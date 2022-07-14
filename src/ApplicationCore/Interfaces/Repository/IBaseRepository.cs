using System.Linq.Expressions;

namespace ApplicationCore.Interfaces.Repository;

public interface IBaseRepository<T> where T : class, IAggregateRoot
{
    IAsyncEnumerable<T> GetAsync(int skip = 0, int take = int.MaxValue);
    
    IAsyncEnumerable<T> GetAsync(Expression<Func<T, bool>> predicate, int skip = 0, int take = int.MaxValue);
    
    IAsyncEnumerable<T> GetSortedAsync<TSortedBy>(Expression<Func<T, TSortedBy>> order, int skip = 0, int take = 2147483647);

    IAsyncEnumerable<T> GetSortedAsync<TSortedBy>(Expression<Func<T, TSortedBy>> order, Expression<Func<T, bool>> predicate,
        int skip = 0, int take = 2147483647);
    
    IAsyncEnumerable<T> GetSortedDescAsync<TSortedBy>(Expression<Func<T, TSortedBy>> order, int skip = 0, int take = 2147483647);
    
    IAsyncEnumerable<T> GetSortedDescAsync<TSortedBy>(Expression<Func<T, TSortedBy>> order, Expression<Func<T, bool>> predicate,
        int skip = 0, int take = 2147483647);
    
    Task<T?> SingleAsync(Expression<Func<T, bool>> predicate);

    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

    Task<int> GetCountAsync();
    
    Task<int> GetCountAsync(Expression<Func<T, bool>> predicate);
    
    Task<T> AddAsync(T entity);
    
    Task UpdateAsync(T entity);
    
    Task DeleteAsync(T entity);
    
    Task SaveChangesAsync();
}