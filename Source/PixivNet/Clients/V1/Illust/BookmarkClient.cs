﻿using System.Collections.Generic;
using System.Threading.Tasks;

using Pixiv.Attributes;
using Pixiv.Extensions;
using Pixiv.Models;

namespace Pixiv.Clients.V1.Illust
{
    public class BookmarkClient : ApiClient
    {
        internal BookmarkClient(PixivClient client) : base(client, "/v1/illust/bookmark") { }

        [ApiVersion]
        [MarkedAs("7.7.7")]
        [RequiredAuthentication]
        public async Task<UserCollection> UsersAsync(long illustId, long? offset = null)
        {
            var parameters = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("illust_id", illustId)
            };
            if (offset.HasValue)
                parameters.Add(new KeyValuePair<string, object>(nameof(offset), offset.Value));

            return await GetAsync<UserCollection>("/users", parameters).Stay();
        }
    }
}