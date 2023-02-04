
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Donations_App.Models;
using DonationsApp.Models;

namespace Donations_App.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<VerifyCode> VerifyCodes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<PatientCase> PatientsCases { get; set; }
    }
}
