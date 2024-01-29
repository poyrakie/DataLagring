using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;

namespace Infrastructure.Repositories.ProductRepositories;

public class OrderRepository(DbFirstDataContext context) : BaseRepository<Order>(context)
{
    private readonly DbFirstDataContext _context = context;
}