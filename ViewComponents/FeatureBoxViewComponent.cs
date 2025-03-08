using Microsoft.AspNetCore.Mvc;

namespace SimoshStore;

public class FeatureBoxViewComponent : ViewComponent
{
    private readonly IBlogService _blogService;
    public FeatureBoxViewComponent(IBlogService blogService)
    {
        _blogService = blogService;
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var blogs = await _blogService.GetAllBlogsAsync();
        return View(blogs);
    }
}
