namespace Todo_Notifier.Models;

public class Todo
{
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? Completed { get; set; }
    public DateTime? Created { get; set; }
}
