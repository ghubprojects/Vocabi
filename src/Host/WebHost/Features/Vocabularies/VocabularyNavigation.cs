using Microsoft.AspNetCore.Components;

namespace WebHost.Features.Vocabularies;

public sealed class VocabularyNavigation(NavigationManager navigation)
{
    public void ToList(bool forceReload = false)
    {
        navigation.NavigateTo(Routes.List(), forceLoad: forceReload);
    }

    public void ToCreate()
    {
        navigation.NavigateTo(Routes.Create());
    }

    public void ToDetail(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty.", nameof(id));

        navigation.NavigateTo(Routes.Detail(id));
    }

    public void ToEdit(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty.", nameof(id));

        navigation.NavigateTo(Routes.Edit(id));
    }
}