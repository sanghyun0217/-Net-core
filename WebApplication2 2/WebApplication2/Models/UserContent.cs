using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Models {
  public class UserContent : DbContext {
    public UserContent(DbContextOptions<UserContent> options)
        : base(options) {
    }

    public DbSet<WebApplication2.Models.Article> Article { get; set; }

  }
}
