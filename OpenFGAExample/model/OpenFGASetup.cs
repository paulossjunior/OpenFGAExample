using System.Text;
using System.Text.Json;


namespace OpenFGAExample.model;

using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class OpenFGASetup
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly string _storeId;

    public string AuthorizationModelId => _authorizationModelId;
    public OpenFGASetup(string baseUrl, string storeId)
    {
        _baseUrl = baseUrl.TrimEnd('/');
        _storeId = storeId;
        _httpClient = new HttpClient();
    }

    public async Task CreateAuthorizationModel()
    {
        var authorizationModel = @"{
            ""schema_version"": ""1.1"",
            ""type_definitions"": [
                {
                    ""type"": ""user"",
                    ""relations"": {}
                },
                {
                    ""type"": ""document"",
                    ""relations"": {
                        ""reader"": {
                            ""this"": {}
                        },
                        ""writer"": {
                            ""this"": {}
                        }
                    },
                    ""metadata"": {
                        ""relations"": {
                            ""reader"": {
                                ""directly_related_user_types"": [
                                    { ""type"": ""user"" }
                                ]
                            },
                            ""writer"": {
                                ""directly_related_user_types"": [
                                    { ""type"": ""user"" }
                                ]
                            }
                        }
                    }
                }
            ]
        }";

        var content = new StringContent(
            authorizationModel,
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync(
            $"{_baseUrl}/stores/{_storeId}/authorization-models",
            content
        );

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to create authorization model: {response.StatusCode}, Error: {errorContent}");
        }

        var result = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Authorization model created successfully: {result}");

        // Extrair e salvar o ID do modelo de autorização
        var modelResponse = JsonSerializer.Deserialize<AuthorizationModelResponse>(result);
        _authorizationModelId = modelResponse.authorization_model_id;
        Console.WriteLine($"Authorization Model ID: {_authorizationModelId}");
    }

    private string _authorizationModelId;

    public async Task CreateRelationships()
    {
        // Formato correto do JSON para criar relacionamentos
        var writeRequestJson = @"{
            ""writes"": {
                ""tuple_keys"": [
                    {
                        ""user"": ""user:john"",
                        ""relation"": ""reader"",
                        ""object"": ""document:doc1""
                    },
                    {
                        ""user"": ""user:jane"",
                        ""relation"": ""writer"",
                        ""object"": ""document:doc1""
                    }
                ]
            },
            ""authorization_model_id"": """ + _authorizationModelId + @"""
        }";

        var content = new StringContent(
            writeRequestJson,
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync(
            $"{_baseUrl}/stores/{_storeId}/write",
            content
        );

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to create relationships: {response.StatusCode}, Error: {errorContent}");
        }

        Console.WriteLine("Relationships created successfully");
    }

    private class AuthorizationModelResponse
    {
        public string authorization_model_id { get; set; }
    }
}
