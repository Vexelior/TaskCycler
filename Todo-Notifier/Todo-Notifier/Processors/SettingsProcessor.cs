using System.Text.Json;

namespace Todo_Notifier.Processors;

public static class SettingsProcessor
{
    public static string? GetConnectionString()
    {
        string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
        if (!File.Exists(configFilePath))
        {
            throw new FileNotFoundException("Configuration file not found.", configFilePath);
        }

        string jsonContent = File.ReadAllText(configFilePath);
        if (string.IsNullOrWhiteSpace(jsonContent))
        {
            throw new InvalidOperationException("Configuration file is empty.");
        }

        using JsonDocument document = JsonDocument.Parse(jsonContent);
        if (!document.RootElement.TryGetProperty("ConnectionString", out JsonElement connectionStringElement))
        {
            throw new InvalidOperationException("Connection string not found in configuration file.");
        }

        if (connectionStringElement.ValueKind != JsonValueKind.String || string.IsNullOrWhiteSpace(connectionStringElement.GetString()))
        {
            throw new InvalidOperationException("Invalid or empty connection string in configuration file.");
        }

        string connectionString = connectionStringElement.GetString()!;

        return connectionString;
    }
}
