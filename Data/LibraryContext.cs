using Microsoft.EntityFrameworkCore;

namespace LibraryReservationSystem.Data
{

public class LibraryContext : DbContext
{
    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

    public DbSet<Book> Books { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
  }
}