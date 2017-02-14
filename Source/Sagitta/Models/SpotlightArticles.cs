﻿using System.Collections.Generic;

using Newtonsoft.Json;

namespace Sagitta.Models
{
    public class SpotlightArticles
    {
        [JsonProperty("spotlight_articles")]
        public IEnumerable<SpotlightArticle> Articles { get; set; }

        [JsonProperty("next_url")]
        public string NextUrl { get; set; }
    }
}