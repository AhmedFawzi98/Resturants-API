using Resturants.Domain.Entities;
using Resturants.Domain.Interfaces.Repositories;
using Resturants.Infrastructure.Persistance;

namespace Resturants.Infrastructure.Repositories;

internal class UnitOfWork : IUnitOfWork
{
    private bool _isDisposed;
    private readonly RestaurantDbContext _context;
    private IGenericRepository<Restaurant> _restaurants;
    private IGenericRepository<Dish> _dishes;


    public UnitOfWork(RestaurantDbContext context)
    {
        _context = context;
    }

    public IGenericRepository<Restaurant> Resturants
    {
        get
        {
            _restaurants ??= new GenericRepository<Restaurant>(_context);
            return _restaurants;
        }
    }
    public IGenericRepository<Dish> Dishes
    {
        get
        {
            _dishes ??= new GenericRepository<Dish>(_context);
            return _dishes;
        }
    }

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    protected void Dispose(bool isDisposing)
    {
        if (_isDisposed)
            return;
        if(isDisposing)
        {
            _context.Dispose();
            _isDisposed = true;
        }
    }
}
