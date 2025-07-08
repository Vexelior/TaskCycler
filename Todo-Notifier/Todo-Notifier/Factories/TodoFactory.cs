using Microsoft.Data.SqlClient;
using Todo_Notifier.Models;
using Todo_Notifier.Processors;

namespace Todo_Notifier.Factories;

public class TodoFactory
{
    private readonly string _connectionString;

    public TodoFactory()
    {
        _connectionString = SettingsProcessor.GetConnectionString() ?? throw new InvalidOperationException("Connection string is not set.");
    }

    public List<Todo> GetTodos()
    {
        List<Todo> todos = new();

        const string queryString = "SELECT Title, IsCompleted, Completed, Created, TimeOfDay FROM Todos";

        using SqlConnection connection = new SqlConnection(_connectionString);
        SqlCommand command = new SqlCommand(queryString, connection);
        connection.Open();

        SqlDataReader reader = command.ExecuteReader();

        try
        {
            while (reader.Read())
            {
                todos.Add(new Todo
                {
                    Title = reader.GetString(0),
                    IsCompleted = reader.GetBoolean(1),
                    Completed = reader.IsDBNull(2) ? null : reader.GetDateTime(2),
                    Created = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                    TimeOfDay = reader.IsDBNull(4) ? null : reader.GetString(4)
                });
            }
        }
        catch
        {
            Console.WriteLine("Error reading from database");
        }
        finally
        {
            reader.Close();
        }

        return todos;
    }

    public void AddTodo(Todo todo)
    {
        const string queryString = "INSERT INTO Todos (Id, Title, Description, IsCompleted, Created, Due, TimeOfDay) VALUES (@Id, @Title, @Description, @IsCompleted, @Created, @Due, @TimeOfDay)";
        using SqlConnection connection = new SqlConnection(_connectionString);
        SqlCommand command = new SqlCommand(queryString, connection);
        command.Parameters.AddWithValue("@Id", Guid.NewGuid());
        command.Parameters.AddWithValue("@Title", todo.Title);
        command.Parameters.AddWithValue("@Description", todo.Description);
        command.Parameters.AddWithValue("@IsCompleted", false);
        command.Parameters.AddWithValue("@Created", DateTime.Today.Date);
        command.Parameters.AddWithValue("@Due", todo.DueDate ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@TimeOfDay", todo.TimeOfDay ?? (object)DBNull.Value);
        connection.Open();
        command.ExecuteNonQuery();
    }

    public bool DeleteTodos()
    {
        const string queryString = "DELETE FROM Todos WHERE IsCompleted = 1";
        using SqlConnection connection = new SqlConnection(_connectionString);
        SqlCommand command = new SqlCommand(queryString, connection);
        connection.Open();
        try
        {
            command.ExecuteNonQuery();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting completed todos: {ex.Message}");
            return false;
        }
    }
}
