using Microsoft.AspNetCore.Mvc;

namespace SimoshStore;

public class SearchBoxViewComponent(IHttpClientFactory httpClientFactory) : ViewComponent
{
    private HttpClient Client => httpClientFactory.CreateClient("Api.Data");

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var response = await Client.GetAsync("/api/search-box");

        if (!response.IsSuccessStatusCode)
        {
            return Content("An error occurred while fetching data from the API.");
        }

        var content = await response.Content.ReadFromJsonAsync<SearchBoxViewModel>();

        var model = new SearchBoxViewModel
        {
            Categories = content.Categories,
            FeaturedProducts = content.FeaturedProducts,
        };

        return View(model);
    }

}
