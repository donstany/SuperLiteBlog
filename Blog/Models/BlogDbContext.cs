using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Blog.Models
{
    public class BlogDbContext : IdentityDbContext<ApplicationUser>
    {
        public BlogDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public virtual IDbSet<Article> Articles { get; set; }

        public static BlogDbContext Create()
        {
            return new BlogDbContext();
        }
        //public System.Data.Entity.DbSet<Blog.Models.ApplicationUser> IdentityUsers { get; set; }
        //public System.Data.Entity.DbSet<Blog.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}