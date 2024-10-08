using EventManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly AplicationDbContext _context;

        public EventController(AplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            return Ok(await _context.Events.Include(e => e.Participants).ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent(int id)
        {
            var evnt = await _context.Events.Include(e => e.Participants).FirstOrDefaultAsync(e => e.Id == id);
            if (evnt == null) return NotFound();
            return Ok(evnt);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent(Event evnt)
        {
            _context.Events.Add(evnt);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEvent), new { id = evnt.Id }, evnt);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, Event evnt)
        {
            if (id != evnt.Id) return BadRequest();
            _context.Entry(evnt).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var evnt = await _context.Events.FindAsync(id);
            if (evnt == null) return NotFound();
            _context.Events.Remove(evnt);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPost("{id}/participantes")]
        public async Task<IActionResult> AddParticipantToEvent(int id, [FromBody] Participant participant)
        {
            var evnt = await _context.Events
                .Include(e => e.Participants)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (evnt == null)
            {
                return NotFound();
            }

            var existingParticipant = await _context.Participants
                .FirstOrDefaultAsync(p => p.Id == participant.Id);

            if (existingParticipant == null)
            {
                return NotFound("El participante no existe.");
            }

            // Verificar si ya está asociado
            if (evnt.Participants.Contains(existingParticipant))
            {
                return BadRequest("El participante ya está asociado a este evento.");
            }

            evnt.Participants.Add(existingParticipant);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE /api/eventos/{id}/participantes/{participantId}: Eliminar un participante de un evento
        [HttpDelete("{id}/participantes/{participantId}")]
        public async Task<IActionResult> RemoveParticipantFromEvent(int id, int participantId)
        {
            var evnt = await _context.Events
                .Include(e => e.Participants)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (evnt == null)
            {
                return NotFound();
            }

            var participant = evnt.Participants.FirstOrDefault(p => p.Id == participantId);
            if (participant == null)
            {
                return NotFound("El participante no está asociado a este evento.");
            }

            evnt.Participants.Remove(participant);
            await _context.SaveChangesAsync();

            return NoContent();

        }
    }
}
