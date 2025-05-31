using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DataAccess.Context
{
    public class MovieAppDbContextFactory : IDesignTimeDbContextFactory<MovieAppDbContext>
    {
        public MovieAppDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<MovieAppDbContext>();
            builder.UseSqlServer("Server=DESKTOP-2V37EJ1\\SQLEXPRESS;Database=MovieAppDb;Trusted_Connection=True;TrustServerCertificate=True");
            return new MovieAppDbContext(builder.Options);
        }
    }
}
