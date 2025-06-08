using Microsoft.AspNetCore.Mvc;
using playlist_pilot.Data;
using playlist_pilot.Models;
using playlist_pilot.DTOs;

namespace playlist_pilot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtistController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ArtistController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CreateArtist(ArtistDTO artistDto)
        {
            var artist = new Artist { ArtistName = artistDto.ArtistName };
            _context.Artists.Add(artist);
            _context.SaveChanges();
            return Ok(artist);
        }

        [HttpGet]
        public IActionResult GetArtists()
        {
            var artists = _context.Artists
                .Select(a => new ArtistDTO
                {
                    ArtistId = a.ArtistId,
                    ArtistName = a.ArtistName
                })
                .ToList();
            return Ok(artists);
        }
        // PUT: api/artist/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateArtist(int id, ArtistDTO artistDto)
        {
            var artist = _context.Artists.Find(id);
            if (artist == null)
            {
                return NotFound();
            }

            artist.ArtistName = artistDto.ArtistName;
            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/artist/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteArtist(int id)
        {
            var artist = _context.Artists.Find(id);
            if (artist == null)
            {
                return NotFound();
            }

            _context.Artists.Remove(artist);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
