using System.Linq.Expressions;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : class, IAggregateRoot
{
    private readonly DbContext _dbContext;
    private readonly DbSet<T> _dbSet;

    protected BaseRepository(DbContext marsContext)
    {
        _dbContext = marsContext;
        _dbSet = marsContext.Set<T>();
    }

    public IAsyncEnumerable<T> GetAsync(int skip = 0, int take = Int32.MaxValue)
    {
        return _dbSet.Skip(skip).Take(take).AsAsyncEnumerable();
    }

    public IAsyncEnumerable<T> GetAsync(Expression<Func<T, bool>> predicate, int skip = 0, int take = Int32.MaxValue)
    {
        return _dbSet
            .Where(predicate)
            .Skip(skip).Take(take)
            .AsAsyncEnumerable();
    }

    public IAsyncEnumerable<T> GetSortedAsync<TSortedBy>(Expression<Func<T, TSortedBy>> order, int skip = 0,
        int take = 2147483647)
    {
        return _dbSet.OrderBy(order).Skip(skip).Take(take).AsAsyncEnumerable();
    }

    public IAsyncEnumerable<T> GetSortedAsync<TSortedBy>(Expression<Func<T, TSortedBy>> order,
        Expression<Func<T, bool>> predicate, int skip = 0, int take = 2147483647)
    {
        return _dbSet.Where(predicate).OrderBy(order).Skip(skip).Take(take).AsAsyncEnumerable();
    }

    public IAsyncEnumerable<T> GetSortedDescAsync<TSortedBy>(Expression<Func<T, TSortedBy>> order, int skip = 0,
        int take = 2147483647)
    {
        return _dbSet.OrderByDescending(order).Skip(skip).Take(take).AsAsyncEnumerable();
    }

    public IAsyncEnumerable<T> GetSortedDescAsync<TSortedBy>(Expression<Func<T, TSortedBy>> order,
        Expression<Func<T, bool>> predicate, int skip = 0, int take = 2147483647)
    {
        return _dbSet.Where(predicate).OrderByDescending(order).Skip(skip).Take(take).AsAsyncEnumerable();
    }

    public async Task<T?> SingleAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.SingleOrDefaultAsync(predicate);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

    public async Task<int> GetCountAsync()
    {
        return await _dbSet.CountAsync();
    }

    public async Task<int> GetCountAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.CountAsync(predicate);
    }

    public async Task<T> AddAsync(T entity)
    {
        _dbSet.Add(entity);

        await SaveChangesAsync();

        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);

        await SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);

        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}