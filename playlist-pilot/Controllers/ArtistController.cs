using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using playlist_pilot.Data;
using playlist_pilot.Models;


namespace playlist_pilot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtistController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ArtistController(ApplicationDbContext context) => _context = context;

        // Inline DTO for Artist
        public record ArtistDto(int ArtistId, string ArtistName);

        /// <summary>
        /// Returns all artists.
        /// </summary>
        /// <returns>200 OK - List of ArtistDto</returns>
        /// <example>GET: api/Artist</example>
        [HttpGet]
        public IActionResult List()
        {
            var artists = _context.Artists
                .Select(a => new ArtistDto(a.ArtistId, a.ArtistName))
                .ToList();

            return Ok(artists);
        }

        /// <summary>
        /// Returns a single artist by ID.
        /// </summary>
        /// <param name="id">The artist ID</param>
        /// <returns>
        /// 200 OK - ArtistDto  
        /// 404 Not Found
        /// </returns>
        /// <example>GET: api/Artist/Find/2</example>
        [HttpGet("Find/{id}")]
        public IActionResult Find(int id)
        {
            var artist = _context.Artists.Find(id);
            if (artist == null) return NotFound();

            return Ok(new ArtistDto(artist.ArtistId, artist.ArtistName));
        }

        /// <summary>
        /// Creates a new artist.
        /// </summary>
        /// <param name="dto">ArtistDto without ArtistId</param>
        /// <returns>201 Created - ArtistDto with new ID</returns>
        /// <example>POST: api/Artist</example>
        [HttpPost]
        public IActionResult Create([FromBody] ArtistDto dto)
        {
            var artist = new Artist { ArtistName = dto.ArtistName };
            _context.Artists.Add(artist);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Find), new { id = artist.ArtistId },
                dto with { ArtistId = artist.ArtistId });
        }

        /// <summary>
        /// Updates an existing artist's name.
        /// </summary>
        /// <param name="id">The artist ID</param>
        /// <param name="dto">Updated ArtistDto</param>
        /// <returns>204 No Content or 404 Not Found</returns>
        /// <example>PUT: api/Artist/2</example>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ArtistDto dto)
        {
            var artist = _context.Artists.Find(id);
            if (artist == null) return NotFound();

            artist.ArtistName = dto.ArtistName;
            _context.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Deletes an artist by ID.
        /// </summary>
        /// <param name="id">Artist ID</param>
        /// <returns>204 No Content or 404 Not Found</returns>
        /// <example>DELETE: api/Artist/3</example>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var artist = _context.Artists
                .Include(a => a.Songs)
                .FirstOrDefault(a => a.ArtistId == id);

            if (artist == null) return NotFound();

            // Optional: Also remove related songs
            if (artist.Songs.Any())
            {
                _context.Songs.RemoveRange(artist.Songs);
            }

            _context.Artists.Remove(artist);
            _context.SaveChanges();

            return NoContent();
        }
    }
}

    

