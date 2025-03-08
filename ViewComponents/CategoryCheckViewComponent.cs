namespace SimoshStore;
using App.Data.Entities;
using Microsoft.AspNetCore.Mvc;

public class CategoryCheckViewComponent(IHttpClientFactory httpClientFactory) : ViewComponent
{
    private HttpClient Client => httpClientFactory.CreateClient("Api.Data");

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var response = await Client.GetAsync("api/categories");
        if (response.IsSuccessStatusCode)
        {
            var categories = await response.Content.ReadFromJsonAsync<List<CategoryEntity>>();
            return View(categories);
        }
        return View(new List<CategoryEntity>());
    }
}
