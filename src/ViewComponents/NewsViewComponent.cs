using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetnepal.Services;
using dotnetnepal.Models;

namespace dotnetnepal.ViewComponents
{
    public class NewsViewComponent : ViewComponent
    {
        private readonly NewsFeedService newsService;

        public NewsViewComponent(NewsFeedService newsService)
        {
            this.newsService = newsService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int quantity)
        {
            var feed = await newsService.GetFeed();
            var items = feed.Take(quantity);

            return View(items);
        }
    }
}
