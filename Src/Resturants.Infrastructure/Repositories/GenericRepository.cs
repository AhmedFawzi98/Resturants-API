using Microsoft.EntityFrameworkCore;
using Resturants.Domain.Enums;
using Resturants.Domain.Interfaces.Repositories;
using Resturants.Infrastructure.Persistance;
using System.Globalization;
using System.Linq.Expressions;

namespace Resturants.Infrastructure.Repositories;

internal class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly RestaurantDbContext _context;

    public GenericRepository(RestaurantDbContext context)
    {
        _context = context;
    }

        public async Task<(IEnumerable<T>, int)> GetAllAsync(int pageSize, int pageNumber, SortingDirection sortingDirection,
            Expression<Func<T, object>> sortingCritrea = null, Expression<Func<T, bool>> FilterCritrea = null, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if(includes != null && includes.Length > 0)
            {
                foreach(string include in includes)
                    query = query.Include(include);
            }

            if(FilterCritrea != null)
                query = query.Where(FilterCritrea);

            int totalCount = await query.CountAsync();

            if(sortingCritrea != null)
            {
                query = sortingDirection == SortingDirection.Ascending ? 
                    query.OrderBy(sortingCritrea) : query.OrderByDescending(sortingCritrea);
            }

            query = query.Skip(pageSize * (pageNumber - 1))
                .Take(pageSize);

            return (await query.ToListAsync(), totalCount);
        }


    public async Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null)
    {
        IQueryable<T> query = _context.Set<T>();
        if (includes != null && includes.Length > 0)
        {
            foreach (string include in includes)
                query = query.Include(include);
        }
        return await query.FirstOrDefaultAsync(criteria);

    }

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public void Delete(T entity)
    {
         _context.Set<T>().Remove(entity);
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    public void DeleteRange(IEnumerable<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
    }
}
