using App.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace SimoshStore;

public class CommentSectionViewComponent(IHttpClientFactory httpClientFactory) : ViewComponent
{
    private HttpClient CLient => httpClientFactory.CreateClient("Api.Data");

    public async Task<IViewComponentResult> InvokeAsync()
{
    var response = await CLient.GetAsync("api/productcomments");
    if (response.IsSuccessStatusCode)
    {
        var comments = await response.Content.ReadFromJsonAsync<List<ProductCommentEntity>>();
        return View(comments);
    }
    return View(new List<ProductCommentEntity>());
}

}

