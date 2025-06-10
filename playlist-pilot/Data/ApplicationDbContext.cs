using Microsoft.EntityFrameworkCore;
using playlist_pilot.Models;

namespace playlist_pilot.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
               : base(options) { }

        public DbSet<Artist> Artists => Set<Artist>();
        public DbSet<Song> Songs => Set<Song>();
        public DbSet<Playlist> Playlists => Set<Playlist>();
        public DbSet<PlaylistSong> PlaylistSongs => Set<PlaylistSong>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlaylistSong>()
                .HasKey(ps => new { ps.PlaylistId, ps.SongId });

            modelBuilder.Entity<PlaylistSong>()
                .HasOne(ps => ps.Playlist)
                .WithMany(p => p.PlaylistSongs)
                .HasForeignKey(ps => ps.PlaylistId);

            modelBuilder.Entity<PlaylistSong>()
                .HasOne(ps => ps.Song)
                .WithMany(s => s.PlaylistSongs)
                .HasForeignKey(ps => ps.SongId);
        }
    }
}
