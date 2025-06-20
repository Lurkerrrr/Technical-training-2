using Microsoft.EntityFrameworkCore;
using ProjectCinema.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCinema.infrastructure
{
    public class CinemaTicketDbContext : DbContext
    {
         public CinemaTicketDbContext()
        { 
        }
        public CinemaTicketDbContext(DbContextOptions<CinemaTicketDbContext> options) : base(options)
        { 
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Seance> Seances { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=cinema;Trusted_connection=True;TrustServerCertificate=True;", x => x.MigrationsHistoryTable("__EFMigrationsHistory", "Cinema"));
        }
    }
}
