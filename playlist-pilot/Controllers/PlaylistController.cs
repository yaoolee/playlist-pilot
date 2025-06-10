using Microsoft.AspNetCore.Mvc;
using playlist_pilot.Data;
using playlist_pilot.Models;
using Microsoft.EntityFrameworkCore;

namespace playlist_pilot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public PlaylistController(ApplicationDbContext context) => _context = context;

        public record PlaylistDto(int PlaylistId, string PlaylistName, List<int> SongIds);

        /// <summary>
        /// Returns all playlists with their associated songs.
        /// </summary>
        /// <returns>200 OK - List of PlaylistDto</returns>
        /// <example>
        /// GET: api/Playlist -> [{PlaylistDto}, ...]
        /// </example>
        [HttpGet]
        public IActionResult List()
        {
            var playlists = _context.Playlists
                .Include(p => p.PlaylistSongs)
                .Select(p => new PlaylistDto(
                    p.PlaylistId,
                    p.PlaylistName,
                    p.PlaylistSongs.Select(ps => ps.SongId).ToList()
                ))
                .ToList();

            return Ok(playlists);
        }

        /// <summary>
        /// Returns a single playlist by ID.
        /// </summary>
        /// <param name="id">Playlist ID</param>
        /// <returns>200 OK or 404 Not Found</returns>
        /// <example>
        /// GET: api/Playlist/Find/1 -> {PlaylistDto}
        /// </example>
        [HttpGet("Find/{id}")]
        public IActionResult Find(int id)
        {
            var playlist = _context.Playlists
                .Include(p => p.PlaylistSongs)
                .FirstOrDefault(p => p.PlaylistId == id);

            if (playlist == null) return NotFound();

            return Ok(new PlaylistDto(
                playlist.PlaylistId,
                playlist.PlaylistName,
                playlist.PlaylistSongs.Select(ps => ps.SongId).ToList()
            ));
        }

        /// <summary>
        /// Creates a new playlist with a list of song IDs.
        /// </summary>
        /// <param name="dto">Playlist data with song IDs</param>
        /// <returns>201 Created</returns>
        /// <example>
        /// POST: api/Playlist
        /// </example>
        [HttpPost]
        public IActionResult Create([FromBody] PlaylistDto dto)
        {
            if (!dto.SongIds.All(id => _context.Songs.Any(s => s.SongId == id)))
                return BadRequest("One or more song IDs are invalid.");

            var playlist = new Playlist
            {
                PlaylistName = dto.PlaylistName,
                PlaylistSongs = dto.SongIds.Select(id => new PlaylistSong
                {
                    SongId = id
                }).ToList()
            };

            _context.Playlists.Add(playlist);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Find), new { id = playlist.PlaylistId },
                dto with { PlaylistId = playlist.PlaylistId });
        }

        /// <summary>
        /// Updates an existing playlist and its associated songs.
        /// </summary>
        /// <param name="id">Playlist ID</param>
        /// <param name="dto">Updated playlist data</param>
        /// <returns>204 No Content</returns>
        /// <example>
        /// PUT: api/Playlist/2
        /// </example>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] PlaylistDto dto)
        {
            var playlist = _context.Playlists
                .Include(p => p.PlaylistSongs)
                .FirstOrDefault(p => p.PlaylistId == id);

            if (playlist == null) return NotFound();

            if (!dto.SongIds.All(id => _context.Songs.Any(s => s.SongId == id)))
                return BadRequest("One or more song IDs are invalid.");

            playlist.PlaylistName = dto.PlaylistName;
            playlist.PlaylistSongs.Clear();

            foreach (var songId in dto.SongIds)
            {
                playlist.PlaylistSongs.Add(new PlaylistSong { SongId = songId });
            }

            _context.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// Deletes a playlist by ID.
        /// </summary>
        /// <param name="id">Playlist ID</param>
        /// <returns>204 No Content</returns>
        /// <example>
        /// DELETE: api/Playlist/3
        /// </example>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var playlist = _context.Playlists.Find(id);
            if (playlist == null) return NotFound();

            _context.Playlists.Remove(playlist);
            _context.SaveChanges();

            return NoContent();
        }
    }
}