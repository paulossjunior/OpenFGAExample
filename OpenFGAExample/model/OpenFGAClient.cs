namespace OpenFGAExample.model;

using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class OpenFGAClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly string _storeId;
    private readonly string _authorizationModelId;

    public OpenFGAClient(string baseUrl, string storeId, string authorizationModelId)
    {
        _baseUrl = baseUrl.TrimEnd('/');
        _storeId = storeId;
        _authorizationModelId = authorizationModelId;
        _httpClient = new HttpClient();
    }

    public async Task<bool> Check(string user, string relation, string objectType, string objectId)
    {
        var checkRequest = @"{
            ""tuple_key"": {
                ""user"": """ + user + @""",
                ""relation"": """ + relation + @""",
                ""object"": """ + objectType + ":" + objectId + @"""
            },
            ""authorization_model_id"": """ + _authorizationModelId + @"""
        }";

        var content = new StringContent(
            checkRequest,
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync(
            $"{_baseUrl}/stores/{_storeId}/check",
            content
        );

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Check request failed: {response.StatusCode}, Error: {errorContent}");
        }

        var result = await JsonSerializer.DeserializeAsync<CheckResponse>(
            await response.Content.ReadAsStreamAsync()
        );

        return result?.allowed ?? false;
    }

    private class CheckResponse
    {
        public bool allowed { get; set; }
    }
}