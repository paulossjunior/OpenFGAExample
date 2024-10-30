// See https://aka.ms/new-console-template for more information

// Initialize the client

using OpenFGAExample.model;

public class Program
{
    public static async Task Main()
    {
        var baseUrl = "http://localhost:8080";
        var storeId = "01JBDDMXDFC06DXSK73RBRQ67M"; // Substitua pelo seu Store ID

        try
        {
            var setup = new OpenFGASetup(baseUrl, storeId);

            // Criar o modelo de autorização
            await setup.CreateAuthorizationModel();
            Console.WriteLine("Modelo de autorização criado com sucesso");

            // Criar relacionamentos de teste
            await setup.CreateRelationships();
            Console.WriteLine("Relacionamentos criados com sucesso");

            // Testar as permissões usando o novo cliente com o ID do modelo
            var client = new OpenFGAClient(baseUrl, storeId, setup.AuthorizationModelId);

            var canRead = await client.Check(
                "user:john",
                "reader",
                "document",
                "doc1"
            );
            Console.WriteLine($"Can John read doc1? {canRead}");

            var canWrite = await client.Check(
                "user:jane",
                "writer",
                "document",
                "doc1"
            );
            Console.WriteLine($"Can Jane write doc1? {canWrite}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
        }
    }
}