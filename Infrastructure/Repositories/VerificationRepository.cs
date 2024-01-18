using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public class VerificationRepository(DataContext context) : BaseRepository<VerificationEntity>(context)
{
    private readonly DataContext _context = context;
}


//using Infrastructure.Contexts;
//using Infrastructure.Entities;
//using Microsoft.EntityFrameworkCore;
//using System.Diagnostics;
//using System.Linq.Expressions;

//namespace Infrastructure.Repositories;

//public class ProductRepository(DataContext context) : BaseRepository<ProductEntity>(context)
//{
//    private readonly DataContext _context = context;

//    public override IEnumerable<ProductEntity> GetAll()
//    {
//        try
//        {
//            return _context.Products.Include(x => x.Category).ToList();
//        }
//        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
//        return null!;
//    }

//    public override ProductEntity GetOne(Expression<Func<ProductEntity, bool>> predicate)
//    {
//        try
//        {
//            return _context.Products.Include(x => x.Category).FirstOrDefault(predicate, null!);
//        }
//        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
//        return null!;
//    }
//}
