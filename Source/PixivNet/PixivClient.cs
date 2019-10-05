﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using Pixiv.Clients;
using Pixiv.Exceptions;
using Pixiv.Extensions;
using Pixiv.Handlers;
using Pixiv.Models;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pixiv
{
    /// <summary>
    ///     ルートクラス, 全ての API はこのクラスから呼び出します。
    /// </summary>
    public class PixivClient
    {
        private readonly HttpClient _httpClient;
        internal static string AppVersion => "7.4.4";
        internal static string OsVersion => "12.1.2";
        internal string ClientId { get; }
        internal string ClientSecret { get; }

        /// <summary>
        ///     現在使用しているアクセストークン
        /// </summary>
        public string AccessToken { get; internal set; }

        /// <summary>
        ///     現在使用しているリフレッシュトークン
        /// </summary>
        public string RefreshToken { get; internal set; }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="clientId">Client ID (ライブラリには含まれまていません)</param>
        /// <param name="clientSecret">Client Secret (ライブラリには含まれていません)</param>
        /// <param name="handler"></param>
        public PixivClient(string clientId, string clientSecret, HttpMessageHandler handler = null)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;

            // 2019/01/28
            _httpClient = new HttpClient(handler ?? new OAuth2HttpClientHandler(this));
            _httpClient.DefaultRequestHeaders.Add("App-OS-Version", OsVersion);
            _httpClient.DefaultRequestHeaders.Add("App-OS", "ios");
            _httpClient.DefaultRequestHeaders.Add("App-Version", AppVersion);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", $"PixivIOSApp/{AppVersion} (iOS {OsVersion}; iPhone11,2)");

            // Initialize accessors
            Application = new ApplicationInfoClient(this);
            Authentication = new AuthenticationClient(this);
            Illust = new IllustClient(this);
            IllustSeries = new IllustSeriesClient(this);
            Live = new LiveClient(this);
            Manga = new MangaClient(this);
            Mute = new MuteClient(this);
            Notification = new NotificationClient(this);
            Novel = new NovelClient(this);
            Search = new SearchClient(this);
            Spotlight = new SpotlightClient(this);
            TrendingTags = new TrendingTagsClient(this);
            User = new UserClient(this);
            Walkthrough = new WalkthroughClient(this);
            File = new FileClient(this);
        }

        internal async Task<T> GetAsync<T>(string url, List<KeyValuePair<string, object>> parameters = null)
        {
            var obj = (await GetAsync(url, parameters).Stay()).ToObject<T>();
            if (obj is ICursorable cursorable)
                cursorable.PixivClient = this;
            return obj;
        }

        internal async Task<JObject> GetAsync(string url, List<KeyValuePair<string, object>> parameters = null)
        {
            if (parameters != null && parameters.Count > 0)
                url += "?" + string.Join("&", parameters.Select(w => $"{w.Key}={Uri.EscapeDataString(AsStringValue(w.Value))}"));
            var response = await _httpClient.GetAsync(url).Stay();
            HandleErrors(response);

            return JObject.Parse(await response.Content.ReadAsStringAsync().Stay());
        }

        internal async Task<T> PostAsync<T>(string url, IEnumerable<KeyValuePair<string, object>> parameters)
        {
            return (await PostAsync(url, parameters).Stay()).ToObject<T>();
        }

        internal async Task<JObject> PostAsync(string url, IEnumerable<KeyValuePair<string, object>> parameters)
        {
            var content = new FormUrlEncodedContent(parameters.Select(w => new KeyValuePair<string, string>(w.Key, AsStringValue(w.Value))));
            var response = await _httpClient.PostAsync(url, content).Stay();
            HandleErrors(response);
            return JObject.Parse(await response.Content.ReadAsStringAsync().Stay());
        }

        private static string AsStringValue<T>(T w)
        {
            return w is bool ? w.ToString().ToLower() : w.ToString();
        }

        private static void HandleErrors(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;
            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedException(response);

                case HttpStatusCode.BadRequest:
                    throw new BadRequestException(response);

                default:
                    response.EnsureSuccessStatusCode();
                    break;
            }
        }

        #region API Accessors

        // ReSharper disable MemberCanBePrivate.Global

        /// <summary>
        ///     アプリケーション情報関連 API へのアクセサー
        /// </summary>
        public ApplicationInfoClient Application { get; }

        /// <summary>
        ///     認証関連 API へのアクセサー
        /// </summary>
        public AuthenticationClient Authentication { get; }

        /// <summary>
        ///     イラスト関連 API へのアクセサー
        /// </summary>
        public IllustClient Illust { get; }

        /// <summary>
        ///     イラストシリーズ関連 API へのアクセサー
        /// </summary>
        public IllustSeriesClient IllustSeries { get; }

        /// <summary>
        ///     生放送関連 API へのアクセサー
        /// </summary>
        public LiveClient Live { get; }

        /// <summary>
        ///     マンガ関連 API へのアクセサー
        /// </summary>
        public MangaClient Manga { get; }

        /// <summary>
        ///     ミュート関連 API へのアクセサー
        /// </summary>
        public MuteClient Mute { get; }

        /// <summary>
        ///     通知関連 API へのアクセサー
        /// </summary>
        public NotificationClient Notification { get; }

        /// <summary>
        ///     小説関連 API へのアクセサー
        /// </summary>
        public NovelClient Novel { get; }

        /// <summary>
        ///     検索関連 API へのアクセサー
        /// </summary>
        public SearchClient Search { get; }

        /// <summary>
        ///     Pixivision (旧 pixiv Spotlight)  関連 API へのアクセサー
        /// </summary>
        public SpotlightClient Spotlight { get; }

        /// <summary>
        ///     トレンドタグ関連 API へのアクセサー
        /// </summary>
        public TrendingTagsClient TrendingTags { get; }

        /// <summary>
        ///     ユーザー関連 API へのアクセサー
        /// </summary>
        public UserClient User { get; }

        /// <summary>
        ///     デモンストレーション関連 API へのアクセサー
        /// </summary>
        public WalkthroughClient Walkthrough { get; }

        /// <summary>
        ///     Pixiv ファイル関連 API へのアクセサー
        /// </summary>
        public FileClient File { get; }

        // ReSharper restore MemberCanBePrivate.Global

        #endregion
    }
}