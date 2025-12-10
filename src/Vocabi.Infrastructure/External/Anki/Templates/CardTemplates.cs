namespace Vocabi.Infrastructure.External.Anki.Templates;

public static class CardTemplates
{
    public static readonly string Front =
        File.ReadAllText("Templates/CardFront.html");

    public static readonly string Back =
        File.ReadAllText("Templates/CardBack.html");

    public static readonly string Style =
        File.ReadAllText("Templates/CardStyle.css");
}