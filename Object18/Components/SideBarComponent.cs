using Microsoft.AspNetCore.Mvc;

namespace Object18.Components;

public class SideBarComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View("/Views/Components/_SideBarComponent.cshtml");
    }
}