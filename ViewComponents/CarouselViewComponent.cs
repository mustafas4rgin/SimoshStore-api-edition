using App.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace SimoshStore;

public class CarouselViewComponent(IHttpClientFactory httpClient) : ViewComponent
{
    private HttpClient Client => httpClient.CreateClient("Api.Data");

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var response = await Client.GetAsync("/api/bestproducts");
        
        if(!response.IsSuccessStatusCode)
        {
            return Content("Data cannot be fetched.");
        }

        var carouselProducts = await response.Content.ReadFromJsonAsync<List<ProductEntity>>();

        return View(carouselProducts);
    }
}
