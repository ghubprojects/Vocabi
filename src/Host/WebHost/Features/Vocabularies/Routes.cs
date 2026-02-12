namespace WebHost.Features.Vocabularies;

public static class Routes
{
    private const string Base = "/vocabularies";

    public static string List() => Base;
    public static string Create() => $"{Base}/create";
    public static string Detail(Guid id) => $"{Base}/{id}";
    public static string Edit(Guid id) => $"{Base}/{id}/edit";
}