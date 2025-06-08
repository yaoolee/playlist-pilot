using Microsoft.AspNetCore.Mvc;
using playlist_pilot.Data;
using playlist_pilot.Models;
using playlist_pilot.DTOs;
using Microsoft.EntityFrameworkCore;

namespace playlist_pilot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlaylistController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CreatePlaylist(PlaylistDTO playlistDto)
        {
            var playlist = new Playlist
            {
                PlaylistName = playlistDto.PlaylistName
            };

            foreach (var songId in playlistDto.SongIds)
            {
                playlist.PlaylistSongs.Add(new PlaylistSong
                {
                    SongId = songId
                });
            }

            _context.Playlists.Add(playlist);
            _context.SaveChanges();
            return Ok(playlist);
        }

        [HttpGet]
        public IActionResult GetPlaylists()
        {
            var playlists = _context.Playlists
                .Include(p => p.PlaylistSongs)
                .ThenInclude(ps => ps.Song)
                .Select(p => new
                {
                    p.PlaylistId,
                    p.PlaylistName,
                    Songs = p.PlaylistSongs.Select(ps => new
                    {
                        ps.Song.SongId,
                        ps.Song.Title,
                        ps.Song.DurationInSeconds
                    })
                })
                .ToList();
            return Ok(playlists);
        }
        // PUT: api/playlist/{id}
        [HttpPut("{id}")]
        public IActionResult UpdatePlaylist(int id, PlaylistDTO playlistDto)
        {
            var playlist = _context.Playlists
                .Include(p => p.PlaylistSongs)
                .FirstOrDefault(p => p.PlaylistId == id);

            if (playlist == null)
            {
                return NotFound();
            }

            playlist.PlaylistName = playlistDto.PlaylistName;

            // Remove existing songs
            playlist.PlaylistSongs.Clear();

            // Add updated songs
            foreach (var songId in playlistDto.SongIds)
            {
                playlist.PlaylistSongs.Add(new PlaylistSong
                {
                    SongId = songId
                });
            }

            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/playlist/{id}
        [HttpDelete("{id}")]
        public IActionResult DeletePlaylist(int id)
        {
            var playlist = _context.Playlists.Find(id);
            if (playlist == null)
            {
                return NotFound();
            }

            _context.Playlists.Remove(playlist);
            _context.SaveChanges();

            return NoContent();
        }
    }
}