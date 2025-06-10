namespace playlist_pilot.Models
{
    public class Song
    {
        public int SongId { get; set; }
        public string Title { get; set; }
        public int DurationInSeconds { get; set; }

        public int ArtistId { get; set; }
        public Artist Artist { get; set; }

        public ICollection<PlaylistSong> PlaylistSongs { get; set; } = new List<PlaylistSong>();

    }
}
