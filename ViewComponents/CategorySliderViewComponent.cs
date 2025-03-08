using App.Data.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace SimoshStore;

public class CategorySliderViewComponent(IHttpClientFactory clientFactory) : ViewComponent
{
    private HttpClient Client => clientFactory.CreateClient("Api.Data");

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var response = await Client.GetAsync("/api/categories");

        if(!response.IsSuccessStatusCode)
        {
            return Content("Data cannot be fetched.");
        }

        var categories = await response.Content.ReadFromJsonAsync<List<CategoryEntity>>();

        return View(categories);
    }
}
