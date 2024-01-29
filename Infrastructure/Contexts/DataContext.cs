using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public partial class DataContext : DbContext
{
    public DataContext()
    {
        
    }
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public virtual DbSet<UserEntity> Users { get; set; }
    public virtual DbSet<AddressEntity> Addresses { get; set; }
    public virtual DbSet<RoleEntity> Roles { get; set; }
    public virtual DbSet<VerificationEntity> Verifications { get; set; }
    public virtual DbSet<ProfileEntity> Profiles { get; set; }


}
