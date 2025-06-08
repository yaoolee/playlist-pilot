using Microsoft.AspNetCore.Mvc;
using playlist_pilot.Data;
using playlist_pilot.Models;
using playlist_pilot.DTOs;

namespace playlist_pilot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SongController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CreateSong(SongDTO songDto)
        {
            var song = new Song
            {
                Title = songDto.Title,
                DurationInSeconds = songDto.DurationInSeconds,
                ArtistId = songDto.ArtistId
            };
            _context.Songs.Add(song);
            _context.SaveChanges();
            return Ok(song);
        }

        [HttpGet]
        public IActionResult GetSongs()
        {
            var songs = _context.Songs
                .Select(s => new SongDTO
                {
                    SongId = s.SongId,
                    Title = s.Title,
                    DurationInSeconds = s.DurationInSeconds,
                    ArtistId = s.ArtistId
                })
                .ToList();
            return Ok(songs);
        }
        // PUT: api/song/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateSong(int id, SongDTO songDto)
        {
            var song = _context.Songs.Find(id);
            if (song == null)
            {
                return NotFound();
            }

            song.Title = songDto.Title;
            song.DurationInSeconds = songDto.DurationInSeconds;
            song.ArtistId = songDto.ArtistId;
            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/song/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteSong(int id)
        {
            var song = _context.Songs.Find(id);
            if (song == null)
            {
                return NotFound();
            }

            _context.Songs.Remove(song);
            _context.SaveChanges();

            return NoContent();
        }

        // GET: api/song/artist/{artistId}
        [HttpGet("artist/{artistId}")]
        public IActionResult GetSongsByArtist(int artistId)
        {
            var songs = _context.Songs
                .Where(s => s.ArtistId == artistId)
                .Select(s => new SongDTO
                {
                    SongId = s.SongId,
                    Title = s.Title,
                    DurationInSeconds = s.DurationInSeconds,
                    ArtistId = s.ArtistId
                })
                .ToList();

            return Ok(songs);
        }
    }
}
