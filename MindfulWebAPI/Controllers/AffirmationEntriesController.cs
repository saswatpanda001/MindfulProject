using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MindfulWebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindfulWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AffirmationEntriesController : ControllerBase
    {
        private readonly MindfulDbContext _context;

        public AffirmationEntriesController(MindfulDbContext context)
        {
            _context = context;
        }

        // GET: api/AffirmationEntries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AffirmationEntry>>> GetAffirmations()
        {
            return await _context.AffirmationEntries.ToListAsync();
        }

        // GET: api/AffirmationEntries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AffirmationEntry>> GetAffirmation(int id)
        {
            var affirmation = await _context.AffirmationEntries
                .FirstOrDefaultAsync(a => a.Id == id);

            if (affirmation == null)
                return NotFound();

            return affirmation;
        }


        [HttpPost]
        public async Task<ActionResult<AffirmationEntry>> CreateAffirmation(AffirmationEntry affirmation)
        {
            _context.AffirmationEntries.Add(affirmation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAffirmation), new { id = affirmation.Id }, affirmation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAffirmation(int id, AffirmationEntry affirmation)
        {
            if (id != affirmation.Id)
                return BadRequest();

            _context.Entry(affirmation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AffirmationExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAffirmation(int id)
        {
            var affirmation = await _context.AffirmationEntries.FindAsync(id);
            if (affirmation == null)
                return NotFound();

            _context.AffirmationEntries.Remove(affirmation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AffirmationExists(int id) => _context.AffirmationEntries.AnyAsync(a => a.Id == id).Result;
    }
}
