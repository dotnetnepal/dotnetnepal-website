﻿using System.Collections.Generic;

namespace dotnetnepal.Models
{
    public class MeetupFeed
    {
        public MeetupFeed()
        {
            Events = new List<MeetupEvent>();
        }

        public List<MeetupEvent> Events { get; set; }
    }
}
