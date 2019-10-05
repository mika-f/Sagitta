﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Pixiv.Extensions;
using Pixiv.Helpers;

namespace Pixiv.Clients.User.BrowsingHistory
{
    /// <summary>
    ///     閲覧履歴 (小説) 関連 API
    /// </summary>
    public class NovelClient : ApiClient
    {
        /// <inheritdoc />
        internal NovelClient(PixivClient pixivClient) : base(pixivClient) { }

        /// <summary>
        ///     指定した小説 ID を閲覧履歴へと追加します。
        /// </summary>
        /// <param name="novelIds">小説 ID</param>
        public async Task AddAsync(long[] novelIds)
        {
            Ensure.ArraySizeNotZero(novelIds, nameof(novelIds));

            var parameters = new List<KeyValuePair<string, object>>();
            parameters.AddRange(parameters.Select(w => new KeyValuePair<string, object>("novel_ids[]", w)));

            await PixivClient.PostAsync("https://app-api.pixiv.net/v2/user/browsing-history/novel/add", parameters).Stay();
        }
    }
}