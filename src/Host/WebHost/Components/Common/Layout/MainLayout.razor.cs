using Microsoft.AspNetCore.Components.Web;

namespace Vocabi.Web.Components.Common.Layout;

public partial class MainLayout
{
    private ErrorBoundary? errorBoundary = default!;

    protected override void OnParametersSet()
    {
        errorBoundary?.Recover();
    }
}