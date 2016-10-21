using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration.Conventions;
namespace AspNetIdentityImplementation.Models
{
    public class ApplicationUser : IdentityUser<int, ApplicationLogin, ApplicationUserRole, ApplicationClaim>
    {
        //custom fields
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        [StringLength(50)]
        public override string Email { get; set; }
        [Required]
        [StringLength(50)]
        public override string UserName { get; set; }
        [StringLength(50)]
        public override string PhoneNumber { get; set; }
        //flag for user status
        public bool IsActive { get; set; }
        public ApplicationUser()
        {
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }
    public class ApplicationUserRole : IdentityUserRole<int>
    {
    }
    public class ApplicationLogin : IdentityUserLogin<int>
    {
        public virtual ApplicationUser User { get; set; }
    }
    public class ApplicationClaim : IdentityUserClaim<int>
    {
        public virtual ApplicationUser User { get; set; }
    }
    public class ApplicationRole : IdentityRole<int, ApplicationUserRole>
    {
    }
    public class ApplicationContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, ApplicationLogin, ApplicationUserRole, ApplicationClaim>
    {
        public ApplicationContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer<ApplicationContext>(new CreateDatabaseIfNotExists<ApplicationContext>());
        }
        public static ApplicationContext Create()
        {
            return new ApplicationContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //custom column names
            modelBuilder.Entity<ApplicationLogin>().Property(x => x.UserId).HasColumnName("UserID");
            modelBuilder.Entity<ApplicationClaim>().Property(x => x.Id).HasColumnName("ID");
            modelBuilder.Entity<ApplicationClaim>().Property(x => x.UserId).HasColumnName("UserID");
            modelBuilder.Entity<ApplicationUser>().Property(x => x.Id).HasColumnName("ID");
            modelBuilder.Entity<ApplicationRole>().Property(x => x.Id).HasColumnName("ID");
            modelBuilder.Entity<ApplicationUserRole>().Property(x => x.UserId).HasColumnName("UserID");
            modelBuilder.Entity<ApplicationUserRole>().Property(x => x.RoleId).HasColumnName("RoleID");
            //custom table names
            modelBuilder.Entity<ApplicationUser>().ToTable("User");
            modelBuilder.Entity<ApplicationRole>().ToTable("Role");
            modelBuilder.Entity<ApplicationUserRole>().ToTable("UserRole");
            modelBuilder.Entity<ApplicationClaim>().ToTable("Claim");
            modelBuilder.Entity<ApplicationLogin>().ToTable("Login");
        }
    }
}