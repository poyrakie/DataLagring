using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;

namespace Infrastructure.Repositories.ProductRepositories;

public class ProductRepository(DbFirstDataContext context) : BaseRepository<Product>(context)
{
    private readonly DbFirstDataContext _context = context;
}