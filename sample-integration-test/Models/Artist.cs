public class Artist
{
    public int ID { get; set; }
    public string ArtistName { get; set; }
    public ICollection<Album> Albums { get; set; }
}