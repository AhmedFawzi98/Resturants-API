using Resturants.Domain.Entities;
using Resturants.Domain.Enums;
using System.Linq.Expressions;

namespace Resturants.Domain.Interfaces.Repositories;

public interface IGenericRepository<T> where T : class
{
    Task<(IEnumerable<T>,int)> GetAllAsync(int pageSize, int pageNumber, SortingDirection sortingDirection,
        Expression<Func<T, object>> sortingCritrea = null, Expression < Func<T, bool>> FilterCritrea = null, string[] includes = null);
    Task<T> FindAsync(Expression<Func<T,bool>> criteria, string[] includes = null);
    Task AddAsync(T entity);
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);
    void Update(T entity);




}
