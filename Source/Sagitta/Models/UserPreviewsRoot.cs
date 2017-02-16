﻿using System.Collections.Generic;

using Newtonsoft.Json;

namespace Sagitta.Models
{
    public class UserPreviewsRoot
    {
        [JsonProperty("user_previews")]
        public IEnumerable<UserPreview> UserPreviews { get; set; }

        [JsonProperty("next_url")]
        public string NextUrl { get; set; }
    }
}