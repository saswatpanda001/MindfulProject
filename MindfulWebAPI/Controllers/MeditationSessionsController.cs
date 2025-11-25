using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MindfulWebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindfulWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeditationSessionsController : ControllerBase
    {
        private readonly MindfulDbContext _context;

        public MeditationSessionsController(MindfulDbContext context)
        {
            _context = context;
        }

        // GET: api/MeditationSessions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MeditationSession>>> GetSessions()
        {
            return await _context.MeditationSessions.ToListAsync();
        }

        // GET: api/MeditationSessions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MeditationSession>> GetSession(int id)
        {
            var session = await _context.MeditationSessions
                .FirstOrDefaultAsync(s => s.Id == id);

            if (session == null)
                return NotFound();

            return session;
        }


        [HttpPost]
        public async Task<ActionResult<MeditationSession>> CreateSession(MeditationSession session)
        {
            _context.MeditationSessions.Add(session);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSession), new { id = session.Id }, session);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSession(int id, MeditationSession session)
        {
            if (id != session.Id)
                return BadRequest();

            _context.Entry(session).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SessionExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSession(int id)
        {
            var session = await _context.MeditationSessions.FindAsync(id);
            if (session == null)
                return NotFound();

            _context.MeditationSessions.Remove(session);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SessionExists(int id) => _context.MeditationSessions.AnyAsync(s => s.Id == id).Result;
    }
}
