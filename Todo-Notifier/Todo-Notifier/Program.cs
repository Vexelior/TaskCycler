using System.Text.Json;
using Todo_Notifier.Factories;
using Todo_Notifier.Models;

namespace Todo_Notifier;

public static class Program
{
    private static void Main()
    {
        Console.WriteLine("Starting Todo Notifier...");

        TodoFactory todoFactory = new TodoFactory();

        List<Todo> todos = todoFactory.GetTodos();

        string json = File.ReadAllText("config.json");
        JsonDocument config = JsonDocument.Parse(json);
        JsonElement.ArrayEnumerator tasks = config.RootElement.GetProperty("Tasks").EnumerateArray();

        if (tasks.Any())
        {
            foreach (JsonElement task in tasks)
            {
                string dayOfTheWeek = task.GetProperty("DayOfTheWeek").GetString();
                if (dayOfTheWeek == DateTime.Now.DayOfWeek.ToString())
                {
                    bool taskExists = todos.Any(t => t.Title == task.GetProperty("Title").GetString() && !t.IsCompleted);
                    if (taskExists)
                    {
                        Console.WriteLine($"Task '{task.GetProperty("Title").GetString()}' already exists. Skipping addition.");
                        continue;
                    }

                    string timeOfDay = task.TryGetProperty("TimeOfDay", out JsonElement timeOfDayElement) 
                        ? timeOfDayElement.GetString() 
                        : string.Empty;

                    Todo todo = new Todo
                    {
                        Title = task.GetProperty("Title").GetString(),
                        Description = task.GetProperty("Description").GetString(),
                        IsCompleted = false,
                        Created = DateTime.Now.Date,
                        DueDate = DateTime.Now.Date,
                        TimeOfDay = timeOfDay
                    };
                    todoFactory.AddTodo(todo);
                }

                if (dayOfTheWeek == "Daily")
                {
                    bool taskExists = todos.Any(t => t.Title == task.GetProperty("Title").GetString() && !t.IsCompleted);
                    if (taskExists)
                    {
                        Console.WriteLine($"Task '{task.GetProperty("Title").GetString()}' already exists. Skipping addition.");
                        continue;
                    }

                    string timeOfDay = task.TryGetProperty("TimeOfDay", out JsonElement timeOfDayElement) 
                        ? timeOfDayElement.GetString() 
                        : string.Empty;

                    Todo todo = new Todo
                    {
                        Title = task.GetProperty("Title").GetString(),
                        Description = task.GetProperty("Description").GetString(),
                        IsCompleted = false,
                        Created = DateTime.Now.Date,
                        DueDate = DateTime.Now.Date,
                        TimeOfDay = timeOfDay
                    };
                    todoFactory.AddTodo(todo);
                }
            }

            Console.WriteLine("Todo Notifier completed successfully.");

            Console.WriteLine("Attempting to delete completed tasks...");
            var batchDeleteIndicator = todoFactory.DeleteTodos();
            if (batchDeleteIndicator)
            {
                Console.WriteLine("Completed tasks deleted successfully.");
            }

            Console.WriteLine("All tasks processed successfully.");
        }
        else
        {
            Console.WriteLine("No tasks found in the configuration file.");
        }
    }
}
