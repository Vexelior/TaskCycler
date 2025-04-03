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

        List<Todo> dailyTodos = new List<Todo>
        {
            new Todo
            {
                Title = "Workout.",
                IsCompleted = false,
                Created = DateTime.Now
            },
            new Todo
            {
                Title = "Feed animals.",
                IsCompleted = false,
                Created = DateTime.Now
            },
            new Todo
            {
                Title = "Read for 30 - 60 minutes.",
                IsCompleted = false,
                Created = DateTime.Now
            }
        };

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