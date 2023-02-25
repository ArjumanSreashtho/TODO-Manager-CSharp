namespace Todo_Manager.DTO.Task;
public class TaskUserDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}

public class TaskDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool Completed { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<TaskUserDTO> Users { get; set; }
}