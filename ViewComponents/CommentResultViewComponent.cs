﻿using Microsoft.AspNetCore.Mvc;

namespace SimoshStore;

public class CommentResultViewComponent(IHttpClientFactory httpClientFactory) : ViewComponent
{
    private HttpClient Client => httpClientFactory.CreateClient("Api.Data");

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var response = await Client.GetAsync("api/result-comments");
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<int>();
            return View(result);
        }
        return View(0);
    }
}
