namespace Vocabi.Infrastructure.External.Image;

public class PixabayResponse
{
    public List<PixabayHit> Hits { get; set; } = [];

    public class PixabayHit
    {
        public string LargeImageURL { get; set; } = string.Empty;
    }
}