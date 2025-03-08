using App.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SimoshStore;

public class BlogSectionViewComponent(IHttpClientFactory httpClientFactory) : ViewComponent
{
    private HttpClient Client => httpClientFactory.CreateClient("Api.Data");

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var response = await Client.GetAsync("/api/blog-section");

        if(!response.IsSuccessStatusCode)
        {
            return Content("Data cannot be fetched.");
        }

        var blogs = await response.Content.ReadFromJsonAsync<List<BlogEntity>>();

        return View(blogs);
    }

}
