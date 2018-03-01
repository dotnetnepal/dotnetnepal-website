﻿using dotnetnepal.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace dotnetnepal.Services
{
    public class NewsFeedService
    {
        public NewsFeedService(
            IOptions<NewsFeedConfig> configOptionsAccessor,
            IMemoryCache cache
            )
        {
            _config = configOptionsAccessor.Value;
            _cache = cache;
        }

        private NewsFeedConfig _config;
        private IMemoryCache _cache;
        private List<NewsItem> _feed = null;

        private async Task<List<NewsItem>> GetFeedInternal()
        {
            var opml = XDocument.Load(_config.OpmlFile);
            var feedData = from item in opml.Descendants("outline")
                select new {
                    Source = (string) item.Attribute("title"),
                    XmlUrl = (string) item.Attribute("xmlUrl")
                };
            
            var feed = new List<NewsItem>();
            foreach (var currentFeed in feedData)
            {
                using (var xmlReader = XmlReader.Create(currentFeed.XmlUrl, new XmlReaderSettings() { Async = true }))
                {
                    var feedReader = new RssFeedReader(xmlReader);

                    while (await feedReader.Read())
                    {
                        if(feedReader.ElementType == SyndicationElementType.Item) 
                        {
                                ISyndicationItem item = await feedReader.ReadItem();
                                feed.Add(new NewsItem {
                                    Title = item.Title,
                                    Uri = item.Links.First().Uri.AbsoluteUri,
                                    Excerpt = item.Description.PlainTextTruncate(120),
                                    PublishDate = item.Published.UtcDateTime,
                                    Source = currentFeed.Source
                                });
                        }
                    }
                }
            }
            return feed.OrderByDescending(f => f.PublishDate).ToList();
        }

        private async Task<List<NewsItem>> GetOrCreateFeedCacheAsync()
        {
            List<NewsItem> result = null;
            if (!_cache.TryGetValue<List<NewsItem>>(_config.CacheKey, out result))
            {
                result = await GetFeedInternal();
                
                if (result != null)
                {
                    _cache.Set(
                        _config.CacheKey,
                        result,
                        new MemoryCacheEntryOptions()
                         .SetSlidingExpiration(TimeSpan.FromSeconds(_config.CacheDurationInSeconds))
                         );
                }

            }

            if (result == null) { throw new InvalidOperationException("failed to retrieve news feed"); }

            return result;
        }

        private async Task EnsureFeed()
        {
            if (_feed == null)
            {
                _feed = await GetOrCreateFeedCacheAsync();
            }

        }

        public async Task<List<NewsItem>> GetFeed()
        {
            await EnsureFeed();
            return _feed;
        }
    }
}
