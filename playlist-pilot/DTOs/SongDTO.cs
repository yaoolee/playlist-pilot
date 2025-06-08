namespace playlist_pilot.DTOs
{
    public class SongDTO
    {
        public int SongId { get; set; }
        public string Title { get; set; }
        public int DurationInSeconds { get; set; }
        public int ArtistId { get; set; }
    }
}
