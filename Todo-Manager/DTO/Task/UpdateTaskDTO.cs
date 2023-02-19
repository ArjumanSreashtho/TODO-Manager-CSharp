namespace Todo_Manager.DTO.Task;

public class UpdateTaskDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool Completed { get; set; }
}