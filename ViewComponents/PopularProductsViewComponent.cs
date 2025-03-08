using App.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace SimoshStore;

public class PopularProductsViewComponent(IHttpClientFactory clientFactory) : ViewComponent
{
    private HttpClient Client => clientFactory.CreateClient("Api.Data");

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var response = await Client.GetAsync("/api/popularproducts?take=8");

        if(!response.IsSuccessStatusCode)
        {
            return Content("Data cannot be fetched.");
        }

        var popularProducts = await response.Content.ReadFromJsonAsync<List<ProductEntity>>();

        return View(popularProducts);
    }

}
