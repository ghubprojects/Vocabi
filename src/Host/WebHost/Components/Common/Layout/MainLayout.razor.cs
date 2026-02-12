using Microsoft.AspNetCore.Components.Web;

namespace WebHost.Components.Common.Layout;

public partial class MainLayout
{
    private ErrorBoundary? errorBoundary = default!;

    protected override void OnParametersSet()
    {
        errorBoundary?.Recover();
    }
}