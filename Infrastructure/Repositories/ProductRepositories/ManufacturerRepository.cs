using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;

namespace Infrastructure.Repositories.ProductRepositories;

public class ManufacturerRepository(DbFirstDataContext context) : BaseRepository<Manufacturer>(context)
{
    private readonly DbFirstDataContext _context = context;
}