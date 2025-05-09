using System.Text.Json;
using Todo_Notifier.Factories;
using Todo_Notifier.Models;
using Todo_Notifier.Services;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Starting Todo Notifier...");

        EmailNotifier emailNotifier = new EmailNotifier();
        TodoFactory todoFactory = new TodoFactory();

        List<Todo> todos = todoFactory.GetTodos();
        List<Todo> inCompleteTodos = todos.FindAll(todo => !todo.IsCompleted);

        if (inCompleteTodos.Count > 0)
        {
            emailNotifier.SendEmail(inCompleteTodos);
        }

        List<Todo> completedTodos = todos.FindAll(todo => todo.IsCompleted);
        if (completedTodos.Count > 0)
        {
            Console.WriteLine("Removing completed todos from database...");
            List<Todo> removedTodos = new List<Todo>();
            foreach (var todo in completedTodos)
            {
                todoFactory.DeleteTodo(todo.Title);
                removedTodos.Add(todo);
            }

            foreach (var removedTodo in removedTodos)
            {
                todos.Remove(removedTodo);
            }
        }

        string json = File.ReadAllText("config.json");
        var config = JsonDocument.Parse(json);
        var tasks = config.RootElement.GetProperty("Tasks").EnumerateArray();

        foreach (var task in tasks)
        {
            string dayOfTheWeek = task.GetProperty("DayOfTheWeek").GetString();
            if (dayOfTheWeek == DateTime.Now.DayOfWeek.ToString())
            {
                Todo todo = new Todo
                {
                    Title = task.GetProperty("Title").GetString(),
                    IsCompleted = false,
                    Created = DateTime.Now,
                };
                todoFactory.AddTodo(todo);
            }

            if (dayOfTheWeek == "Daily")
            {
                bool taskExists = todos.Any(t => t.Title == task.GetProperty("Title").GetString());
                if (taskExists)
                {
                    Console.WriteLine($"Task '{task.GetProperty("Title").GetString()}' already exists. Skipping addition.");
                    continue;
                }
                Todo todo = new Todo
                {
                    Title = task.GetProperty("Title").GetString(),
                    IsCompleted = false,
                    Created = DateTime.Now,
                };
                todoFactory.AddTodo(todo);
            }
        }

        Console.WriteLine("Todo Notifier completed successfully.");
    }
}