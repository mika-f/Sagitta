﻿using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using Pixiv.Attributes;

namespace Pixiv.Models
{
    public class LossesCollection : ApiResponse
    {
#pragma warning disable CS8618 // Null 非許容フィールドは初期化されていません。null 許容として宣言することを検討してください。

        [ApiVersion]
        [MarkedAs("7.7.7")]
        [JsonProperty("losses")]
        public IEnumerable<ApiResponse> Losses { get; set; }

        [ApiVersion]
        [MarkedAs("7.7.7")]
        [JsonProperty("next_url")]
        public Uri? NextUrl { get; set; }

#pragma warning restore CS8618 // Null 非許容フィールドは初期化されていません。null 許容として宣言することを検討してください。
    }
}