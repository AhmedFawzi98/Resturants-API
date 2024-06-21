using Resturants.Domain.Entities;

namespace Resturants.Domain.Interfaces.Repositories;

public interface IUnitOfWork:IDisposable
{
    public IGenericRepository<Restaurant> Resturants { get;}
    public IGenericRepository<Dish> Dishes { get; }

    Task<int> CommitAsync();
}
