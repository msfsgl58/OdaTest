using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OdaTest.API.Models;

namespace OdaTest.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController(AppDbContext _context) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
        {
            //var reservationMember = _context.Members
            //    .Include(r => r)
            //    .ToList();

            //var reservationRoom = _context.Reservations
            //    .Include(r => r.Rooms)
            //    .ToList();
            return await _context.Reservations.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            return reservation;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutReservation(int id, Reservation reservation)
        {
            if (id != reservation.ReservationId)
            {
                return BadRequest();
            }

            _context.Entry(reservation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Reservation>> PostReservation(Reservation reservation)
        {

            int MemberId = reservation.MemberId;
            string MemberName = reservation.MemberName;
            string MemberSurname = reservation.MemberSurname;

            int RoomId = reservation.RoomId;
            string Name = reservation.RoomName;

            var RegistereMember = await _context.Members.FirstOrDefaultAsync(r => r.MemberId == MemberId && r.MemberName == MemberName && r.MemberSurname == MemberSurname);
            var RegistereRoom = await _context.Rooms.FirstOrDefaultAsync(m => m.RoomId == RoomId && m.Name == Name);
            
            if (RegistereMember == null)
            {
                return NotFound("Kayıtlı Kullanıcı Bulunamadı");
            }

            int MemberReservationCount = RegistereMember.MemberReservation;
            if (MemberReservationCount >= 2)
            {
                return NotFound("Sadece 2 Adet Oda Rezerve Edebilirsiniz");
            }

            if (RegistereRoom == null)
            {
                return NotFound("Kayıtlı Oda Bulunamadı");
            }
            if (RegistereRoom.RoomReservation)
            {
                return NotFound("Oda Dolu");
            }

            _context.Reservations.Add(reservation);
            RegistereRoom.RoomReservation = true;
            RegistereMember.MemberReservation += 1;
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetReservation", new { id = reservation.ReservationId }, reservation);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.ReservationId == id);
        }
    }
}
