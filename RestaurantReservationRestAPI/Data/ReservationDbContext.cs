using Microsoft.EntityFrameworkCore;
using RestaurantReservationRestAPI.Models;

namespace RestaurantReservationRestAPI.Data
{
    public class ReservationDbContext : DbContext
    {
        public ReservationDbContext(DbContextOptions<ReservationDbContext> options):base(options)
        {

        }

        public DbSet<Reservation> Reservations { get; set; }
    }
}
