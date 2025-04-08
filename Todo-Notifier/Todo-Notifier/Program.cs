using System.Text.Json;
using Org.BouncyCastle.Math.EC;
using Todo_Notifier;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Starting Todo Notifier...");

        EmailNotifier emailNotifier = new EmailNotifier();
        List<Todo> todos = TodoFactory.GetTodos();
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
                TodoFactory.DeleteTodo(todo.Title);
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

        List<Todo> dailyTodos = tasks.Select(task => new Todo
        {
            Title = task.GetProperty("Title").GetString(), 
            IsCompleted = task.GetProperty("IsCompleted").GetBoolean(), 
            Created = DateTime.Now
        }).ToList();

        if (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday)
        {
            dailyTodos.Add(new Todo
            {
                Title = "Take out trash.",
                IsCompleted = false,
                Created = DateTime.Now
            });
        }

        if (dailyTodos.Count > 0)
        {
            foreach (var dailyTodo in dailyTodos.Where(dailyTodo => !todos.Exists(todo => todo.Title == dailyTodo.Title)))
            {
                TodoFactory.AddTodo(dailyTodo);
            }
        }

        Console.WriteLine("Todo Notifier completed successfully.");
    }
}