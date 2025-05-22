namespace Todo_Notifier.Models;

public class Todo
{
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? Completed { get; set; }
    public DateTime? Created { get; set; }
    public string DayOfTheWeek { get; set; }
    public DateTime? DueDate { get; set; }
}
