using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MindfulWebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindfulWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoodEntriesController : ControllerBase
    {
        private readonly MindfulDbContext _context;

        public MoodEntriesController(MindfulDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MoodEntry>>> GetMoodEntries()
        {
            return await _context.MoodEntries.ToListAsync();
        }

        // GET: api/MoodEntries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MoodEntry>> GetMoodEntry(int id)
        {
            var moodEntry = await _context.MoodEntries
                .FirstOrDefaultAsync(m => m.Id == id);

            if (moodEntry == null)
                return NotFound();

            return moodEntry;
        }


        // POST: api/MoodEntries
        [HttpPost]
        public async Task<ActionResult<MoodEntry>> CreateMoodEntry(MoodEntry moodEntry)
        {
            _context.MoodEntries.Add(moodEntry);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMoodEntry), new { id = moodEntry.Id }, moodEntry);
        }

        // PUT: api/MoodEntries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMoodEntry(int id, MoodEntry moodEntry)
        {
            if (id != moodEntry.Id)
                return BadRequest();

            _context.Entry(moodEntry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MoodEntryExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/MoodEntries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMoodEntry(int id)
        {
            var moodEntry = await _context.MoodEntries.FindAsync(id);
            if (moodEntry == null)
                return NotFound();

            _context.MoodEntries.Remove(moodEntry);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MoodEntryExists(int id) => _context.MoodEntries.AnyAsync(m => m.Id == id).Result;
    }
}
