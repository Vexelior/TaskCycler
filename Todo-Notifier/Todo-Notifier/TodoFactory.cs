using Microsoft.Data.SqlClient;

namespace Todo_Notifier;

public class TodoFactory
{
    private const string connectionString = "Data Source=192.168.0.198,1433;Initial Catalog=Todo;Persist Security Info=False;User ID=Alex;Password=MyKelsi41512#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";

    public static List<Todo> GetTodos()
    {
        List<Todo> todos = [];

        const string queryString = "SELECT Title, IsCompleted, Completed, Created FROM Todos";

        using SqlConnection connection = new SqlConnection(connectionString);
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
                    Created = reader.IsDBNull(3) ? null : reader.GetDateTime(3)
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

    public static void AddTodo(Todo todo)
    {
        const string queryString = "INSERT INTO Todos (Title, IsCompleted, Completed, Created) VALUES (@Title, @IsCompleted, @Completed, @Created)";
        using SqlConnection connection = new SqlConnection(connectionString);
        SqlCommand command = new SqlCommand(queryString, connection);
        command.Parameters.AddWithValue("@Title", todo.Title);
        command.Parameters.AddWithValue("@IsCompleted", todo.IsCompleted);
        command.Parameters.AddWithValue("@Completed", (object)todo.Completed ?? DBNull.Value);
        command.Parameters.AddWithValue("@Created", (object)todo.Created ?? DBNull.Value);
        connection.Open();
        command.ExecuteNonQuery();
    }

    public static void DeleteTodo(string title)
    {
        const string queryString = "DELETE FROM Todos WHERE Title = @Title";
        using SqlConnection connection = new SqlConnection(connectionString);
        SqlCommand command = new SqlCommand(queryString, connection);
        command.Parameters.AddWithValue("@Title", title);
        connection.Open();
        command.ExecuteNonQuery();
    }
}