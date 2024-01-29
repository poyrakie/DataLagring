using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;

namespace Infrastructure.Repositories.ProductRepositories;

public class OrderRowRepository(DbFirstDataContext context) : BaseRepository<OrderRow>(context)
{
    private readonly DbFirstDataContext _context = context;
}