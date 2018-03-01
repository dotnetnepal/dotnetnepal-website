﻿using Microsoft.SyndicationFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetnepal.Models
{
    public class NewsItem
    {
        public ISyndicationItem ItemDetails { get; set; }
        public string Source { get; set; }
        public string Title { get; set; }
        public string Uri { get; set; }
        public string Excerpt { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
