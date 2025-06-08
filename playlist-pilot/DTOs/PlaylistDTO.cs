namespace playlist_pilot.DTOs
{
    public class PlaylistDTO
    {
        public int PlaylistId { get; set; }
        public string PlaylistName { get; set; }
        public List<int> SongIds { get; set; }
    }
}
