using Microsoft.AspNetCore.Mvc;
using playlist_pilot.Data;
using playlist_pilot.Models;


namespace playlist_pilot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public SongController(ApplicationDbContext context) => _context = context;

        public record SongDto(int SongId, string Title, int DurationInSeconds, int ArtistId);

        /// <summary>
        /// Returns a list of Songs, each represented by a SongDto with its associated ArtistId.
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{SongDto},{SongDto},..]
        /// </returns>
        /// <example>
        /// GET: api/Song -> [{SongDto},{SongDto},..]
        /// </example>
        [HttpGet]
        public IActionResult List()
        {
            var songs = _context.Songs
                .Select(s => new SongDto(s.SongId, s.Title, s.DurationInSeconds, s.ArtistId))
                .ToList();

            return Ok(songs);
        }

        /// <summary>
        /// Returns a single Song by its ID.
        /// </summary>
        /// <param name="id">The song ID</param>
        /// <returns>
        /// 200 OK - SongDto
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// GET: api/Song/1 -> {SongDto}
        /// </example>
        [HttpGet("{id}")]
        public IActionResult Find(int id)
        {
            var song = _context.Songs.Find(id);
            if (song == null) return NotFound();

            return Ok(new SongDto(song.SongId, song.Title, song.DurationInSeconds, song.ArtistId));
        }

        /// <summary>
        /// Creates a new Song with Artist reference.
        /// </summary>
        /// <param name="dto">SongDto without SongId</param>
        /// <returns>201 Created</returns>
        /// <example>
        /// POST: api/Song
        /// </example>
        [HttpPost]
        public IActionResult Create([FromBody] SongDto dto)
        {
            var artistExists = _context.Artists.Any(a => a.ArtistId == dto.ArtistId);
            if (!artistExists) return BadRequest($"Artist with ID {dto.ArtistId} not found.");

            var song = new Song
            {
                Title = dto.Title,
                DurationInSeconds = dto.DurationInSeconds,
                ArtistId = dto.ArtistId
            };

            _context.Songs.Add(song);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Find), new { id = song.SongId }, dto with { SongId = song.SongId });
        }

        /// <summary>
        /// Deletes a song by its ID.
        /// </summary>
        /// <param name="id">Song ID</param>
        /// <returns>204 No Content</returns>
        /// <example>
        /// DELETE: api/Song/3
        /// </example>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var song = _context.Songs.Find(id);
            if (song == null) return NotFound();

            _context.Songs.Remove(song);
            _context.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Returns a list of all songs by a given Artist ID.
        /// </summary>
        /// <param name="artistId">Artist ID</param>
        /// <returns>200 OK - List of SongDto</returns>
        /// <example>
        /// GET: api/Song/ByArtist/1
        /// </example>
        [HttpGet("ByArtist/{artistId}")]
        public IActionResult GetByArtist(int artistId)
        {
            var songs = _context.Songs
                .Where(s => s.ArtistId == artistId)
                .Select(s => new SongDto(s.SongId, s.Title, s.DurationInSeconds, s.ArtistId))
                .ToList();

            return Ok(songs);
        }
    }
}
