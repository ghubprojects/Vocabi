using Microsoft.AspNetCore.Components.Web;

namespace Vocabi.Web.Components.Layout;

public partial class MainLayout
{
    private ErrorBoundary? errorBoundary { set; get; }

    protected override void OnParametersSet()
    {
        errorBoundary?.Recover();
    }
}