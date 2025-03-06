using Todo_Notifier;

internal class Program
{
    private static void Main()
    {
        EmailNotifier emailNotifier = new EmailNotifier();

        List<Todo> potentialTodos = emailNotifier.GetTodos().FindAll(todo => !todo.IsCompleted);

        if (potentialTodos.Count > 0)
        {
            emailNotifier.SendEmail(potentialTodos);
        }
    }
}