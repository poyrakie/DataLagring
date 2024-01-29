using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Entities.ProductEntities;

namespace Infrastructure.Repositories.ProductRepositories;

public class CategoryRepository(DbFirstDataContext context) : BaseRepository<Category>(context)
{
    private readonly DbFirstDataContext _context = context;
}