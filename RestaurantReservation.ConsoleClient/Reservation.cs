using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.ConsoleClient
{
    internal class Reservation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TableFor { get; set; }
        public int TableNo { get; set; }
        public DateTime Time { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
