﻿using Microsoft.AspNetCore.WebUtilities;

namespace Infra.CrossCutting.Externals.Bases
{
    public class BaseExternal
    {
        internal readonly HttpClient _client;

        internal BaseExternal(string baseAddress, IHttpClientFactory client)
        {
            _client = client.CreateClient();
            _client.BaseAddress = new Uri(baseAddress);
            _client.Timeout = TimeSpan.FromSeconds(30);
        }

        internal async Task<string?> Get(string methodName, Dictionary<string, string?> queryParams, bool useQueryParams = true)
        {
            HttpResponseMessage response;

            if (queryParams != null)
            {
                string uri;

                if (useQueryParams)
                    uri = QueryHelpers.AddQueryString(methodName, queryParams);
                else
                    uri = GetPathParamUri(methodName, queryParams);

                response = await _client.GetAsync(uri);
            }
            else
                response = await _client.GetAsync(methodName);

            if (response == null || !response.IsSuccessStatusCode)
                throw new HttpRequestException("ErrorCommunicatingWithExternalService");

            return await response.Content.ReadAsStringAsync();
        }

        private static string GetPathParamUri(string methodName, Dictionary<string, string?> queryParams)
        {
            var uri = methodName;

            foreach (var queryParam in queryParams)
                uri = uri + $"/{queryParam.Value}";

            return uri;
        }
    }
}