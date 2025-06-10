namespace playlist_pilot.Models
{
    public class Playlist
    {
        public int PlaylistId { get; set; }
        public string PlaylistName { get; set; }
        public ICollection<PlaylistSong> PlaylistSongs { get; set; } = new List<PlaylistSong>();
    }
}
