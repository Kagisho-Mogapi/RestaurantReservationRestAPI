using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantReservationRestAPI.Data;
using RestaurantReservationRestAPI.Models;

namespace RestaurantReservationRestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly ReservationDbContext _context;

        public ReservationsController(ReservationDbContext context)
        {
            _context = context;
        }

        [HttpGet("All")]
        public async Task<IEnumerable<Reservation>> AllReservations()
        {
            return await _context.Reservations.ToListAsync();
        }

        [HttpGet("Details/{Id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var res = await _context.Reservations.FindAsync(id);

            if(res == null)
                return NotFound();

            return Ok(res);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(int id, Reservation reservation)
        {
            if (id != reservation.Id)
                return BadRequest();

            _context.Entry(reservation).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Reservation reservation = await _context.Reservations.FindAsync(id);

            if (reservation == null)
                return NotFound();

            _context.Remove(reservation);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
