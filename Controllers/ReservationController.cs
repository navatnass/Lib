using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryReservationSystem.Data;


namespace LibraryReservationSystem

{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly LibraryContext _context;

        public ReservationsController(LibraryContext context)
        {
            _context = context;
        }

        // POST /api/reservations
        [HttpPost]
        public async Task<ActionResult<Reservation>> PostReservation(Reservation reservation)
        {
            // Validate reservation input
            if (reservation.Days < 1)
            {
                return BadRequest("Days must be at least 1.");
            }

            // Fetch the book to ensure it exists
            var book = await _context.Books.FindAsync(reservation.BookId);
            if (book == null)
            {
                return NotFound("Book not found.");
            }

            // Calculate the cost based on type and days
            decimal baseCost = book.Type == "Audiobook" ? 3m : 2m;
            decimal totalCost = baseCost * reservation.Days;

            // Apply discounts
            if (reservation.Days > 10)
            {
                totalCost *= 0.8m; // 20% off for more than 10 days
            }
            else if (reservation.Days > 3)
            {
                totalCost *= 0.9m; // 10% off for more than 3 days
            }

            // Add service fee
            totalCost += 3; // Service fee

            // Add quick pick-up fee if selected
            if (reservation.QuickPickup)
            {
                totalCost += 5; // Quick pick-up fee
            }

            // Create the reservation
            reservation.TotalCost = totalCost; // Assuming you have a TotalCost property in your Reservation model
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, reservation);
        }

        // GET /api/reservations
        [HttpGet]
        public ActionResult<IEnumerable<Reservation>> GetReservations()
        {
            var reservations = _context.Reservations
                .Include(r => r.Book) // Include Book details
                .ToList();
            return Ok(reservations);
        }

        // Optional: GET a single reservation by ID
        [HttpGet("{id}")]
        public ActionResult<Reservation> GetReservation(int id)
        {
            var reservation = _context.Reservations
                .Include(r => r.Book) // Include Book details
                .FirstOrDefault(r => r.Id == id);
            if (reservation == null)
            {
                return NotFound("Reservation not found.");
            }

            return Ok(reservation);
        }
    }
}